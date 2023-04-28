using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class M08tarebGBTController : ControllerBase
    {

        [HttpGet]
        public  IActionResult M08tarebGBTAsync([FromQuery] string query)
        {
            string result = string.Empty;
            var openAi = new OpenAIAPI("");
            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = query;
            completionRequest.Model = OpenAI_API.Models.Model.DavinciText;

            // Set the temperature and max tokens for the response
            completionRequest.Temperature = 0.7;
            completionRequest.MaxTokens = 2048;
            completionRequest.TopP = 1; // Higher top_p = more diversity

            var completions = openAi.Completions.CreateCompletionAsync(completionRequest);
            foreach (var completion in completions.Result.Completions)
            {
                result += completion.Text;
            }
            return Ok(result);
        }


    }
}
