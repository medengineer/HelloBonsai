using Bonsai;
using Bonsai.Expressions;
using Bonsai.Reactive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BonsaiPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "testcounter.bonsai";

            var serializer = WorkflowBuilder.Serializer;
            using (var reader = XmlReader.Create(fileName))
            {
                var workflowBuilder = (WorkflowBuilder)serializer.Deserialize(reader);

                var workflow = workflowBuilder.Workflow.ToInspectableGraph();

                foreach (var node in workflow)
                {
                    var element = ExpressionBuilder.GetWorkflowElement(node.Value);
                    var typeDescriptor = TypeDescriptor.GetProperties(element);

                    //var property = typeDescriptor["DueTime"];
                    //var converter = property.Converter;
                    //property.SetValue(element, converter.ConvertFromString("00:00:01"));

                    Console.WriteLine(typeDescriptor);
                }

                //var skip = new Skip();
                //var combinatorBuilder = new CombinatorBuilder { Combinator = skip };

                //var n = workflowBuilder.Workflow.Add(combinatorBuilder);
                //workflowBuilder.Workflow.AddEdge(n, n, new ExpressionBuilderArgument(0));

                var expression = workflow.BuildObservable();
                foreach (var node in workflow)
                {
                    var inspectBuilder = (InspectBuilder)node.Value;
                    var values = Observable.Merge(inspectBuilder.Output).Subscribe(x => Console.WriteLine(x));
                }

                expression.Subscribe();
                Console.ReadLine();

                Console.WriteLine(workflowBuilder);
            }
        }
    }
}
