// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using templater.contracts;


TemplaterRequest r = new TemplaterRequest(OutputFormats.PDF, zip: false)
{
    Templates = new Template[]
    {
        new Template(applicationId: "xxx", templateId: "mx1")
        {
            Replacements = new[]
            {
                new Replacement("name", "туалетный утёнок"),
                new Replacement("summa", 123.45, ReplacementOptions.ToSumStringOption)
            }
        },
        new Template()
    }
};

Console.WriteLine(r.ToJsonString());
