document.addEventListener("DOMContentLoaded", function () {   
    const newBoardForm = document.getElementById("createBoardForm");
    const saveBoardBtn = document.getElementById("saveBoardBtn");
    const msgBox = document.getElementById("msgBox");
    function showMsg(text, ok) {
        msgBox.classList.remove("hidden");
        msgBox.classList.remove("ok", "err");
        msgBox.classList.add(ok ? "ok" : "err");
        msgBox.innerText = text;
        
        setTimeout(() => {
            msgBox.classList.add("hidden");
        }, 4000);
    }
    
    saveBoardBtn.addEventListener("click", async function () {

        const name = document.getElementById("newBoardName").value.trim();
        const code = document.getElementById("newBoardCode").value.trim();
        const file = document.getElementById("newBoardFile").files[0];

        if (!name) {
            showMsg("Введите название платы", false);
            return;
        }
        if (!code) {
            showMsg("Введите код платы", false);
            return;
        }
        if (!file) {
            showMsg("Выберите файл PDF/DOCX", false);
            return;
        }
        const formData = new FormData();
        formData.append("nameBoard", name);
        formData.append("codeBoard", code);
        formData.append("file", file);
        try {
            const response = await fetch('/Directories/AddNewBoard', {
                method: 'POST',
                body: formData
            });
            const result = await response.json();
            if (response.ok && result.success) {
                showMsg("Плата успешно добавлена!", true);
                
                document.getElementById("newBoardName").value = "";
                document.getElementById("newBoardCode").value = "";
                document.getElementById("newBoardFile").value = "";
            } else {
                showMsg("!!" + (result.message || "Ошибка сервера"), false);
            }

        } catch (error) {
            showMsg("Ошибка соединения: " + error.message, false);
        }
    });
});