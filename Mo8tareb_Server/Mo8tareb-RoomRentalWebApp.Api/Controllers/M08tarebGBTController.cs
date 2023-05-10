using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;
using Org.BouncyCastle.Asn1.Ocsp;

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
            var openAi = new OpenAIAPI("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");
            //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
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
            // Serialize the response data to JSON format
            var responseData = new
            {
                result = result
            };
            var json = JsonConvert.SerializeObject(responseData);

            // Return the JSON response
            return Ok(json);
        }


    }
}
