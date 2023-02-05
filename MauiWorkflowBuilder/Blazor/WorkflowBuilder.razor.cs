namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IronBlock.Blocks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
public class RunWorkflowMessage
{
}

public class ResultWorkflowMessage
{
	public object? Result { get; init; }
}

public partial class WorkflowBuilder : ComponentBase, IDisposable
{
	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	public WorkflowBuilder()
	{
		WeakReferenceMessenger.Default.Register<RunWorkflowMessage>(this, async (r, m) =>
		{
			var xml = await JsRuntime.InvokeAsync<string>("getXml");
			var workspace = new IronBlock.Parser()
							.AddStandardBlocks()
							.Parse(xml);
			var result = workspace.Evaluate();
			WeakReferenceMessenger.Default.Send<ResultWorkflowMessage>(new ResultWorkflowMessage()
			{
				Result = result
			});
		});
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);
		if (firstRender)
		{
			var defaultProgram = await GetDefaultProgram();
			await JsRuntime.InvokeVoidAsync("InitWorkflow", PrepareToolbox(), defaultProgram);
		}
	}

	async Task<string> GetDefaultProgram()
	{
		var defaultProgramStream = await FileSystem.OpenAppPackageFileAsync("wwwroot/defaultProgram.xml");
		var reader = new StreamReader(defaultProgramStream);
		return await reader.ReadToEndAsync();
	}

	string PrepareToolbox()
	{
		var toolbox = new Xml
		{
			Category = new()
			{
				new Category()
				{
					Name = "Logic",
					Colour = "210",
					Block = new List<Block>()
					{
						new Block()
						{
							Type = "controls_if"
						},
						new Block()
						{
							Type = "logic_compare"
						}
					}
				},
				new Category()
				{
					Name = "Loops",
					Colour = "120",
					Block = new List<Block>()
					{
						new Block()
						{
							Type = "controls_repeat_ext",
							Value = new List<Value>()
							{
								new Value()
								{
									Name = "TIMES",
									Shadow = new Shadow()
									{
										Type = "math_number",
										Field = new Field()
										{
											Name = "NUM",
											Text = "10"
										}
									}
								}
							}
						},
						new Block()
						{
							Type = "controls_whileUntil"
						}
					}
				}
			}
		};
		var serializer = new XmlSerializer(typeof(Xml));
		using var writer = new StringWriter();
		serializer.Serialize(writer, toolbox);
		return writer.ToString();
	}

	public void Dispose()
	{
		WeakReferenceMessenger.Default.Unregister<RunWorkflowMessage>(this);
	}
}