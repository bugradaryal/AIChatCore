using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public readonly IMessageServices _messageServices;
        public readonly ISessionServices _sessionServices;
        public MessageController(IMessageServices messageManager, ISessionServices sessionServices)
        {
            _messageServices = messageManager;
            _sessionServices = sessionServices;
        }

        [HttpPost("MessageHistory")]
        public async Task<IActionResult> MessageHistory([FromBody] int sessionId)
        {
            return Ok(await _messageServices.GetChatHistoryAsync(sessionId));
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] DTOSendMessage sendMessage)
        {
            var result = await _messageServices.SendMessageAsync(sendMessage);
            return Ok(result);
        }

        [HttpDelete("DeleteSession/{sessionId}")]
        public async Task<IActionResult> DeleteSession([FromRoute] int sessionId)
        {
            await _sessionServices.DeleteSessionAsync(sessionId);
            return Ok();
        }

        [HttpPost("GetSectionList")]
        public async Task<IActionResult> GetSectionList()
        {
            return Ok(await _sessionServices.GetSectionListAsync());
        }
    }
}
