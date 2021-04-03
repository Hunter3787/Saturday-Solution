using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
/**
* AutoBuild Recommendation Tool Controller.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class RecommendationController
    {
        public RecommendationController()
        {


        }
    }
}
