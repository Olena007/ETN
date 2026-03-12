using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Articles;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class ArticleController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetArticles.ArticlesListDto>> GetAll([FromQuery] GetArticles.GetArticlesQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetArticle.ArticleDto>> Get(Guid id)
    {
        var query = new GetArticle.GetArticleQuery
        {
            Id = id
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    [HttpPost]
    public async Task<ActionResult<ImportArticlesFromFolder.ImportResult>> Import([FromBody] ImportArticlesFromFolder.ImportArticlesCommand command) {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("languages")]
    public async Task<ActionResult<List<string>>> GetLanguages() {
        var result = await Mediator.Send(new GetLanguages.GetLanguagesQuery());
        return Ok(result);
    }
    
    [HttpDelete("language/{language}")]
    public async Task<ActionResult<int>> DeleteByLanguage(string language)
    {
        var result = await Mediator.Send(new DeleteArticlesByLanguage.DeleteArticlesByLanguageCommand { Language = language });
        return Ok(new { deletedCount = result, language = language });
    }
}