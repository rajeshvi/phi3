using Microsoft.AspNetCore.Mvc;

using Microsoft.SemanticKernel;

namespace phi3.Controllers
{
    public class phi3Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> getphi3(String userInput) 
        {
#pragma warning disable SKEXP0010
            var endpoint = new Uri("http://localhost:11434");
            var modelId = "phi3";
            var kernelBuilder = Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId: modelId, apiKey: null, endpoint: endpoint);
            var kernel = kernelBuilder.Build();
            var history = "";
            //var userInput = "Hi, I'm looking for book suggestions";
            string skPrompt = @"
ChatBot can have a conversation with you about any topic.
It can give explicit instructions or say 'I don't know' if it does not have an answer.";
            skPrompt += history;
            skPrompt += "User: " + userInput;
            skPrompt += "ChatBot:";
            var chatFunction = kernel.CreateFunctionFromPrompt(skPrompt);
            var arguments = new KernelArguments()
            {
                ["history"] = history
            };
            arguments["userInput"] = userInput;
            var bot_answer = await chatFunction.InvokeAsync(kernel, arguments);
            history += $"\nUser: "+userInput+"\nAI: "+bot_answer+"\n";
            return Json(history);
        }
    }
}
