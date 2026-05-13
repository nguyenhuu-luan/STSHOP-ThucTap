const chatbotToggle = document.getElementById("chatbot-toggle");
const chatbotBox = document.getElementById("chatbot-box");
const closeChatbot = document.getElementById("close-chatbot");
const sendChatbot = document.getElementById("send-chatbot");
const chatbotInput = document.getElementById("chatbot-input");
const chatbotBody = document.getElementById("chatbot-body");

chatbotToggle.addEventListener("click", () => {

    chatbotBox.style.display = "flex";

    // chỉ load 1 lần
    if (!chatbotBox.dataset.loaded) {

        loadRootMenu();

        chatbotBox.dataset.loaded = "true";
    }
});

closeChatbot.addEventListener("click", () => {
    chatbotBox.style.display = "none";
});


async function sendMessage() {

const message = chatbotInput.value.trim();

if (message === "") return;

// USER MESSAGE
chatbotBody.innerHTML += `
        <div class="user-message">
           ${ message }
        </div >
        `;

chatbotInput.value = "";

scrollChatBottom();

// LOADING
chatbotBody.innerHTML += `
        <div class="bot-message" id="ai-loading">
            Đang trả lời...
        </div>
        `;

scrollChatBottom();

try {

    const response = await fetch('/ChatBot/AskAI', {

        method: 'POST',

        headers: {
            'Content-Type': 'application/json'
        },

        body: JSON.stringify({
            message: message
        })
    });

    const data = await response.json();

    document.getElementById("ai-loading")?.remove();

    chatbotBody.innerHTML += `
        <div class="bot-message">
            ${ data.message }
        </div >
        `;

} catch {

    document.getElementById("ai-loading")?.remove();

    chatbotBody.innerHTML += `
        <div class="bot-message">
            Không thể kết nối AI.
        </div >
        `;
}

scrollChatBottom();

}

// CLICK SEND BUTTON
sendChatbot.addEventListener("click", sendMessage);

// ENTER KEY
chatbotInput.addEventListener("keypress", function (e) {

if (e.key === "Enter") {

    sendMessage();
}

});



async function loadRootMenu() {

    const response =
        await fetch('/ChatBot/GetMenu');

    const data =
        await response.json();

    renderOptions(data.options);
}

function renderOptions(options) {

    let html = `
        <div class="faq-options">
    `;

    options.forEach(option => {

        html += `
            <button class="faq-btn"
                onclick="selectOption(${option.id}, '${option.text}')">

                ${option.text}

            </button>
        `;
    });

    html += `</div>`;

    chatbotBody.innerHTML += `
        <div class="bot-message">
            ${html}
        </div>
    `;

    scrollChatBottom();
}


async function selectOption(id, text) {

    chatbotBody.innerHTML += `
        <div class="user-message">
            ${text}

        </div>
    `;

    scrollChatBottom();

    const response =
        await fetch(`/ChatBot/SelectOption?id=${id}`);

    const data =
        await response.json();

    if (data.message) {

        chatbotBody.innerHTML += `
            <div class="bot-message">
                ${data.message}
            </div>
        `;
    }

    if (data.hasOptions) {

        renderOptions(data.options);
    }

    scrollChatBottom();
}

function scrollChatBottom() {

    chatbotBody.scrollTop =
        chatbotBody.scrollHeight;
}



