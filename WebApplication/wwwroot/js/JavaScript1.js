var b1 = false;//
var b2 = false;// переменные для блокировки кнопки (когда не все поля заполнены)

window.onload = function () {
    block1(document.getElementById('name').value);
    block2(password.value);
}
function block2(input)//функция проверки заполенности поля password (onkeyup)
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

function block1(input)//функция проверки заполенности поля Email (onkeyup)
{
    var value = input
    const reg = new RegExp('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$')
    if (reg.test(value)) {
        b1 = true;
    }
    else {
        b1 = false;
    }
    block();
}


function block()// функция проверки заполненности всех полей
{
    if (b1 && b2) {// каждая переменная b отвечает за заполненность определенного поле
        document.getElementById('Button1').removeAttribute('disabled');

    }
    else {
        document.getElementById('Button1').setAttribute('disabled', 'disabled');
    }
}