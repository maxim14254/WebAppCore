var b1 = false;//
var b2 = false;// переменные для блокировки кнопки (когда не все поля заполнены)
var b3 = false;//
var b4 = false;//
var b5 = false;//

window.onload = function () {
    if (document.all.item('18').textContent == "Добавить фильм") {
        content.innerHTML = "";
    }
    block1(file.defaultValue);
    block2(document.getElementById("name").value);
    block3(year.value);
    block4(producer.value);
    block5(content.innerHTML);
}
function block1(input)
{
    var value = input;
    if (value != '') {
        b1 = true;
    }
    else {
        b1 = false;
    }
    block();
}

function block2(input)
{
    var value = input;
    if (value != '') {
        b2 = true;
    }
    else {
        b2 = false;
    }
    block();
}

function block3(input)
{
    if (input != '')// если поле ИНН пустое блокируем кнопку
    {
        b3 = true;

    }
    else {
        b3 = false;

    }
    block();// функция проверки заполенности всех полей
}

function block4(input) {
    var value = input;
    if (value != '') {
        b4 = true;
    }
    else {
        b4 = false;
    }
    block();
}

function block5(input) {
    var value = input;
    if (value != '') {
        b5 = true;
    }
    else {
        b5 = false;
    }
    block();
}

function block()// функция проверки заполненности всех полей
{
    if (b1 && b2 && b3 && b4 && b5) {// каждая переменная b отвечает за заполненность определенного поле
        document.getElementById('Button1').removeAttribute('disabled');

    }
    else {
        document.getElementById('Button1').setAttribute('disabled', 'disabled');
    }
}