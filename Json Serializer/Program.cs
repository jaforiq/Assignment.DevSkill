using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

public class Instructor
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class Topic
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class AdmissionTest
{
    public string TestName { get; set; }
    public int MaximumScore { get; set; }
}

public class Course
{
    public string Title { get; set; }
    public Instructor Teacher { get; set; }
    public List<Topic> Topics { get; set; }
    public double Fees { get; set; }
    public List<AdmissionTest> Tests { get; set; }
}

public static class JsonFormatter
{
    public static string Converter(object item)
    {
        return Serialize(item);
    }

    private static string Serialize(object obj)
    {
        if (obj == null) return "null";

        Type type = obj.GetType();

        if(type.IsPrimitive || obj is string)
        {
            return SerializePrimitive(obj);
        }

        if(type.IsArray || obj is IEnumerable)
        {
            return SerializeCollection((IEnumerable)obj);
        }

        return SerializeObject(obj);
    }

    private static string SerializePrimitive(object obj)
    {
        if(obj is string str)
        {
            return $"\"{str}\"";
        }

        if(obj is bool b)
        {
            return b.ToString().ToLower();
        }

        return obj.ToString();
    }

    private static string SerializeCollection(IEnumerable collection)
    {
        var sb = new StringBuilder();
        sb.Append("[");

        bool first = true;
        foreach (var item in collection)
        {
            if (!first) sb.Append(",");

            sb.Append(Serialize(item));
            first = false;
        }

        sb.Append("]");
        return sb.ToString();
    }

    private static string SerializeObject(object obj)
    {
        var sb = new StringBuilder();
        sb.Append("{");

        PropertyInfo[] properties = obj.GetType().GetProperties();

        bool first = true;
        foreach (PropertyInfo property in properties)
        {
            if (!property.CanRead) continue;

            if (!first) sb.Append(",");
            sb.Append($"\"{property.Name}\":{Serialize(property.GetValue(obj))}");

            first = false;
        }

        sb.Append("}");
        return sb.ToString();
    }

    public class Program
    {
        public static void Main()
        {
            var instructor = new Instructor
            {
                Name = "John Doe",
                Email = "john.doe@example.com"
            };
           
            var topics = new List<Topic>
            {
            new Topic { Title = "Introduction to C#", Description = "Basics of C# programming" },
            new Topic { Title = "Advanced C#", Description = "In-depth C# concepts" }
            };
           
            var tests = new List<AdmissionTest>
            {
            new AdmissionTest { TestName = "Entrance Exam", MaximumScore = 100 },
            new AdmissionTest { TestName = "Final Exam", MaximumScore = 200 }
            };

            var course = new Course
            {
                Title = "C# Programming",
                Teacher = instructor,
                Topics = topics,
                Fees = 1000.25,
                Tests = tests
            };

            string jsonString = JsonFormatter.Converter(course);

            Console.WriteLine(jsonString);
        }
    }
}
