//Функция отображения ошибки
function ViewError(text) {
    //Получаем элементы
    var error = document.getElementById("error");

    //Делаем ошибку видимой
    error.style.display = 'inherit';

    //Устанавливаем текст
    error.innerHTML = text;
}

//Функция проверки переменной
function Check(object) {
    if (object == undefined || object == null || object.value == undefined || object.value == null || object.value == "") return false;
    return true;
}

//Функция создания ссылки
function CreateUrl(api, versionNumber, controller, action) {
    //Получаем хост
    var hosts = [
        "https://localhost:44312", //web локальный
        //"https://localhost", //web внешний
        "https://192.168.1.109" //api внешний
    ];
    var host;
    if (api) host = hosts[1];
    else host = hosts[0];

    //Получаем версию
    var webVersions = ["/web/v1"];
    var apiVersions = ["/api/v1"];
    var version;
    if (api) version = apiVersions[versionNumber-1];
    else version = webVersions[versionNumber-1];

    //Формируем ссылку
    var url = host + version + '/' + controller + '/' + action;

    //Возвращаем ссылку
    return url;
}