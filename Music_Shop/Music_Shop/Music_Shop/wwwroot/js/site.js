// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Функция для открытия/закрытия меню
function toggleMenu() {
    const menu = document.getElementById('dropdownMenu');
    menu.classList.toggle('show');
}

// Закрытие меню при клике вне его
document.addEventListener('click', function (event) {
    const menu = document.getElementById('dropdownMenu');
    const userIcon = document.querySelector('.user-icon');

    if (!menu.contains(event.target) && event.target !== userIcon) {
        menu.classList.remove('show');
    }
});


