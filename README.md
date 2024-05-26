# Insania
Проект сервиса Инсании

## Стандарты кода

### Наименование таблиц

- **Cтиль** - используется camelCase для наименований таблиц и полей
- **Префиксы**
  - *re* - реестр, основной рабочий инструмент с частоизменяемыми данными
  - *dir* - справочник, хранит редко изменяемые данные, служащие для краткого информационного обозначения
  - *sys* - системные данные, практически неизменяемые данные, не видимые пользователю и служащие для внутреннего использования
  - *un* - объединения, служат для связи таблиц

### XAML
  * xlmns
  * x:Name/x:Uid
  * Grid.Row/Column
  * IsVisible

  * Horizontal/Vertical Options
  * Margin/Padding
  * Widght/Height

  * Row/Column Definitions
  * Orientations
  * Wrap
  * JustifyContent
  * CornerRadius
  * SelectionMode
  * SelectionChanged

  * Backgrounds
  * FontFamily
  * FontSize
  * TextColor

  * Image/Text
  * Style

  * Loaded    
  * Clicked 

### CS
* Атрибуты
* Конструкторы
* События окна/страницы
* События элементов
* Методы
 
## Развёртывание

### Создание новой базы
  - Делаем миграцию *add-migration Init-2 -context ApplicationContext*
  - Обновляем базу *update-database -context ApplicationContext*