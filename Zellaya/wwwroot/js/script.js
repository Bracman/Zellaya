// Функция для изменения контента
function changeContent(section) {
    let content = '';
    switch (section) {
        case 'orders':
            content = document.getElementById('ordersContent').outerHTML;
            break;
        case 'knowledgeBase':
            content = '<p>База знаний</p>';
            break;
        case 'warehouse':
            content = '<p>Кладовая</p>';
            break;
        case 'calculator':
            content = '<p>Калькулятор</p>';
            break;
        case 'settings':
            content = '<p>Настройки</p>';
            break;
        case 'exit':
            content = '<p>Выход из системы</p>';
            break;
        default:
            content = '<p>Выберите раздел</p>';
    }

    document.getElementById('mainContent').innerHTML = content;

    // Закрыть панель на мобильных устройствах, если она открыта
    if (window.innerWidth <= 767) {
        closeMobileSidebar(); 
    }
}

// Функция для отображения формы создания заказа
function showCreateOrderForm() {
    // Скрываем текущий контент (список действий)
    document.getElementById('ordersContent').style.display = 'none';
    // Показываем форму для создания заказа
    document.getElementById('createOrderForm').style.display = 'block';
}

// Функция для сохранения заказа
function saveOrder() {
    // Получаем данные формы
    const orderId = document.getElementById('orderId').value;
    const orderDate = document.getElementById('orderDate').value;
    const creator = document.getElementById('creator').value;
    const contractNumber = document.getElementById('contractNumber').value;
    const paymentCount = document.getElementById('paymentCount').value;
    const paymentIds = document.getElementById('paymentIds').value;
    const paymentNames = document.getElementById('paymentNames').value;

    // Здесь можно добавить код для отправки данных на сервер или обработки их
    alert(`Заказ ${orderId} сохранен!\nДата: ${orderDate}\nКто создал: ${creator}\nНомер договора: ${contractNumber}\nКоличество плат: ${paymentCount}\nID плат: ${paymentIds}\nНазвания плат: ${paymentNames}`);

    // Возвращаемся к основному контенту
    document.getElementById('createOrderForm').style.display = 'none';
    document.getElementById('ordersContent').style.display = 'block';
}

// Скрипт для управления гамбургером на мобильных устройствах
const hamburger = document.getElementById('hamburger');
const mobileSidebar = document.getElementById('mobileSidebar');
const closeBtn = document.getElementById('closeBtn');

hamburger.addEventListener('click', () => {
    mobileSidebar.classList.add('active');
});

closeBtn.addEventListener('click', () => {
    mobileSidebar.classList.remove('active');
});

// Функция для закрытия мобильной панели навигации
function closeMobileSidebar() {
    mobileSidebar.classList.remove('active');
}

window.onload = function() {
    changeContent('orders');
}
