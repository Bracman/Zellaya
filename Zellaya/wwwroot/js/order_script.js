// Функция для динамического создания полей для плат
document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded - initializing scripts');

    // Настраиваем форму новой платы
    setupNewBoardForm();

    // Генерируем начальные поля
    generateBoardFields();
    generateComponentFields();

    // Вешаем обработчики на инпуты количества
    const boardQuantityInput = document.getElementById('BoardQuantity');
    const componentQuantityInput = document.getElementById('ComponentQuantity');

    if (boardQuantityInput) {
        boardQuantityInput.addEventListener('change', generateBoardFields);
        boardQuantityInput.addEventListener('input', generateBoardFields);
    }

    if (componentQuantityInput) {
        componentQuantityInput.addEventListener('change', generateComponentFields);
        componentQuantityInput.addEventListener('input', generateComponentFields);
    }

    console.log('Scripts initialized successfully');
});

function generateBoardFields() {
    const boardQuantity = document.getElementById('BoardQuantity').value;
    const container = document.getElementById('boardFieldsContainer');

    if (!container) {
        console.error('boardFieldsContainer not found');
        return;
    }

    container.innerHTML = '';

    console.log('Generating', boardQuantity, 'board fields');
    console.log('Boards data:', boardsData);

    for (let i = 0; i < boardQuantity; i++) {
        const label = document.createElement('label');
        label.innerText = `Плата ${i + 1}:`;
        label.htmlFor = `boardSelect_${i}`;

        const select = document.createElement('select');
        select.name = `SelectedBoards[${i}]`;
        select.id = `boardSelect_${i}`;
        select.required = true;

        // Создаем базовую опцию
        const defaultOption = document.createElement('option');
        defaultOption.value = "";
        defaultOption.textContent = "— выберите —";
        defaultOption.disabled = true;
        defaultOption.selected = true;
        select.appendChild(defaultOption);

        // Добавляем опции из boardsData
        if (typeof boardsData !== 'undefined' && boardsData.length > 0) {
            boardsData.forEach(board => {
                const option = document.createElement('option');
                option.value = board.value;
                option.textContent = board.text;
                select.appendChild(option);
            });
        } else {
            console.warn('No boards data available');
            const noDataOption = document.createElement('option');
            noDataOption.value = "";
            noDataOption.textContent = "— нет доступных плат —";
            noDataOption.disabled = true;
            select.appendChild(noDataOption);
        }

        const div = document.createElement('div');
        div.classList.add('form-item');
        div.appendChild(label);
        div.appendChild(select);
        container.appendChild(div);
    }
}

// Функция для динамического создания полей для компонентов
function generateComponentFields() {
    const componentQuantity = document.getElementById('ComponentQuantity').value;
    const container = document.getElementById('componentFieldsContainer');

    if (!container) {
        console.error('componentFieldsContainer not found');
        return;
    }

    container.innerHTML = '';

    console.log('Generating', componentQuantity, 'component fields');
    console.log('Components data:', componentsData);

    for (let i = 0; i < componentQuantity; i++) {
        const div = document.createElement('div');
        div.classList.add('form-item');

        // Label для компонента
        const label = document.createElement('label');
        label.innerText = `Компонент ${i + 1}:`;
        label.htmlFor = `componentSelect_${i}`;

        // Select для выбора компонента
        const select = document.createElement('select');
        select.name = `ComponentLines[${i}].ComponentId`; // Исправлено имя
        select.id = `componentSelect_${i}`;
        select.required = true;

        // Базовая опция
        const defaultOption = document.createElement('option');
        defaultOption.value = "";
        defaultOption.textContent = "— выберите —";
        defaultOption.disabled = true;
        defaultOption.selected = true;
        select.appendChild(defaultOption);

        // Добавляем опции из componentsData
        if (typeof componentsData !== 'undefined' && componentsData.length > 0) {
            componentsData.forEach(component => {
                const option = document.createElement('option');
                option.value = component.value;
                option.textContent = component.text;
                select.appendChild(option);
            });
        } else {
            console.warn('No components data available');
            const noDataOption = document.createElement('option');
            noDataOption.value = "";
            noDataOption.textContent = "— нет компонентов —";
            noDataOption.disabled = true;
            select.appendChild(noDataOption);
        }

        // Label для количества
        const quantityLabel = document.createElement('label');
        quantityLabel.innerText = `Количество:`;
        quantityLabel.htmlFor = `componentQuantity_${i}`;

        // Input для количества
        const quantityInput = document.createElement('input');
        quantityInput.type = "number";
        quantityInput.name = `ComponentLines[${i}].Quantity`; // Исправлено имя
        quantityInput.id = `componentQuantity_${i}`;
        quantityInput.min = "1";
        quantityInput.value = "1";
        quantityInput.required = true;
        quantityInput.classList.add('form-control');

        // Собираем все вместе
        div.appendChild(label);
        div.appendChild(select);
        div.appendChild(quantityLabel);
        div.appendChild(quantityInput);
        container.appendChild(div);
    }
}

// Обработчик для кнопки сохранения платы
function setupNewBoardForm() {
    const checkbox = document.getElementById("addNewBoardCheckbox");
    const newBoardForm = document.getElementById("newBoardForm");
    const saveNewBoardBtn = document.getElementById("saveNewBoardBtn");

    console.log('Setup new board form:', { checkbox, newBoardForm, saveNewBoardBtn });

    if (!checkbox || !newBoardForm || !saveNewBoardBtn) {
        console.error('❌ Не найдены элементы формы новой платы');
        return;
    }

    // Обработчик чекбокса
    checkbox.addEventListener("change", function () {
        console.log('Checkbox changed:', this.checked);
        if (this.checked) {
            newBoardForm.classList.remove("hidden");
        } else {
            newBoardForm.classList.add("hidden");
        }
    });

    // Обработчик кнопки сохранения платы
    saveNewBoardBtn.addEventListener("click", async function () {
        console.log('Save board button clicked');

        const name = document.getElementById("newBoardName").value.trim();
        const code = document.getElementById("newBoardCode").value.trim();
        const file = document.getElementById("newBoardFile").files[0];

        console.log('Form data:', { name, code, file });

        if (!name) {
            alert("Введите название платы");
            return;
        }

        if (!code) {
            alert("Введите код платы");
            return;
        }

        const formData = new FormData();
        formData.append("nameBoard", name);
        formData.append("codeBoard", code);
        if (file) {
            formData.append("file", file);
        }

        try {
            console.log('Sending request...');
            const response = await fetch('/Orders/AddNewBoard', {
                method: 'POST',
                body: formData
            });

            console.log('Response status:', response.status);

            if (response.ok) {
                const result = await response.json();
                console.log('Response result:', result);

                if (result.success) {
                    alert("Плата успешно добавлена!");
                    // Очищаем форму
                    document.getElementById("newBoardName").value = "";
                    document.getElementById("newBoardCode").value = "";
                    document.getElementById("newBoardFile").value = "";
                    // Скрываем форму
                    checkbox.checked = false;
                    newBoardForm.classList.add("hidden");
                    // Перезагружаем страницу
                    location.reload();
                } else {
                    const errorMessage = result.message || "Неизвестная ошибка сервера";
                    alert("Произошла ошибка при добавлении платы: " + errorMessage);
                }
            } else {
                try {
                    const errorResult = await response.json();
                    const errorMessage = errorResult.message || response.statusText;
                    alert("Ошибка сервера: " + errorMessage);
                } catch {
                    alert("Ошибка сервера: " + response.statusText);
                }
            }
        } catch (error) {
            console.error("Ошибка:", error);
            alert("Произошла ошибка при отправке запроса: " + error.message);
        }
    });
}