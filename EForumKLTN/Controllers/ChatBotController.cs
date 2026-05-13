using EForumKLTN.Models;
using EForumKLTN.ViewModels.ChatBot;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using EForumKLTN.Prompts;

namespace EForumKLTN.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly EForumContext _context;

        public ChatBotController(EForumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMenu(int? parentId)
        {
            var menus = _context.ChatBotFaQs
                .Where(x =>
                    x.ParentId == parentId
                    && x.IsActive)
                .OrderBy(x => x.ThuTu)
                .ToList();

            var response = new ChatBotVM
            {
                HasOptions = true,

                Options = menus.Select(x =>
                    new ChatBotOptionVM
                    {
                        Id = x.Id,
                        Text = x.TieuDe
                    }).ToList()
            };

            return Json(response);
        }

        [HttpGet]
        public IActionResult SelectOption(int id)
        {
            var item = _context.ChatBotFaQs
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Json(new ChatBotVM
                {
                    Message = "Không tìm thấy dữ liệu."
                });
            }

            // Nếu là menu cha
            if (item.IsMenu)
            {
                var subMenus = _context.ChatBotFaQs
                    .Where(x =>
                        x.ParentId == item.Id
                        && x.IsActive)
                    .OrderBy(x => x.ThuTu)
                    .ToList();

                return Json(new ChatBotVM
                {
                    Message = item.TieuDe,

                    HasOptions = true,

                    Options = subMenus.Select(x =>
                        new ChatBotOptionVM
                        {
                            Id = x.Id,
                            Text = x.TieuDe
                        }).ToList()
                });
            }

            // Nếu là câu trả lời cuối
            return Json(new ChatBotVM
            {
                Message = item.NoiDungTraLoi,
                HasOptions = false
            });
        }




        #region call api model AI

        [HttpPost]
        public async Task<IActionResult> AskAI(
        [FromBody] ChatBotRequestVM request)
        {
            try
            {
                using var client = new HttpClient();

    client.DefaultRequestHeaders.Add(
        "Authorization",
        "Bearer ");
                //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 - nhet vao bearer là chay dc nha e:))
                //up nhanh mot tao file rieng chưa key ko push len git 

                var body = new
                {
                    model = "openai/gpt-3.5-turbo",

                    messages = new[]
                    {
            new
            {
                role = "system",
                content = ChatBotPrompt.SystemPrompt
            },
            new
            {
                role = "user",
                content = request.Message
            }
        }
                };

                var json =
                    JsonSerializer.Serialize(body);

                var content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                var response =
                    await client.PostAsync(
                        "https://openrouter.ai/api/v1/chat/completions",
                        content);

                // đọc raw response trước
                var responseString =
                    await response.Content.ReadAsStringAsync();

                // debug lỗi thật
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = responseString
                    });
                }

                using var doc =
                    JsonDocument.Parse(responseString);

                var aiMessage =
                    doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                return Json(new
                {
                    success = true,
                    message = aiMessage
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
}

        #endregion

    }
}
