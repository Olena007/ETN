using System.Globalization;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Articles;

namespace WebApi.Controllers;

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
}