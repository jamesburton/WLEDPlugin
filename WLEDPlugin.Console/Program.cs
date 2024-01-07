// Based from https://learn.microsoft.com/en-us/semantic-kernel/overview/#how-many-agents-does-it-take-to-change-a-lightbulb

using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

// Create kernel
var builder = Kernel.CreateBuilder();
// Add a text or chat completion service using either:
// builder.Services.AddAzureOpenAIChatCompletion();
// builder.Services.AddAzureOpenAITextGeneration();
// builder.Services.AddOpenAIChatCompletion();
// builder.Services.AddOpenAITextGeneration();
// builder.Plugins.AddFromType<LightPlugin>();

// Add OpenAI chat completion service
var serviceId = "WLEDPlugin.Console";
var modelId = "gpt-3.5-turbo";
// var modelId = "gpt-4";
var openAIClient = new OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
builder.Services.AddOpenAIChatCompletion(modelId, openAIClient, serviceId);

builder.Plugins.AddFromType<WLEDPlugin.WLEDPlugin>();
var kernel = builder.Build();

// Create chat history
ChatHistory history = [];

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Start the conversation
while (true)
{
    // Get user input
    Console.Write("User > ");
    history.AddUserMessage(Console.ReadLine()!);

    // Enable auto function calling
    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content);
}