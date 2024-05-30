# Insania
Проект сервиса Инсании

## Стандарты кода

### Наименование таблиц

- **Cтиль** - используется snake_case для наименований таблиц и полей
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
  * IsEnabled
  * IsRunning
  * IsChecked

  * Horizontal/Vertical Options
  * Margin/Padding
  * Widght/Height
  * Spacing

  * Row/Column Definitions
  * Orientations
  * Wrap
  * JustifyContent
  * CornerRadius
  * StrokeThickness
  * StrokeShape
  * SelectionMode
  * SelectionChanged

  * Background
  * FontFamily
  * FontSize
  * TextColor
  * TitleColor
  * PlaceholderColor
  * Color

  * Image/Text/Placeholder/ItemsSource/Value
  * ItemDisplayBinding
  * Title
  * IsPassword
  * IsToggled
  * ClearButtonVisibility
  * Format
  * Minimum/Maximum
  * Keyboard
  * Aspect
  * AutoSize
  * Style

  * Loaded    
  * Clicked
  * SelectedIndexChanged

### CS
* Атрибуты
* Конструкторы
* События окна/страницы
* События элементов
* Методы
 
## Развёртывание

### Создание новой базы
  - Делаем миграцию *add-migration Init -context ApplicationContext*
  - Обновляем базу *update-database -context ApplicationContext*