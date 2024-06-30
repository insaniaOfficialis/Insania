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
  * Grid.Row/ColumnSpan
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
  * Stroke
  * StrokeShape
  * StrokeThickness
  * SelectionMode
  * SelectionChanged

  * Background
  * FontFamily
  * FontSize
  * TextColor
  * TitleColor
  * PlaceholderColor
  * Color

  * Image/Text/Placeholder/ItemsSource/Value/SelectedIndex
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
  * PropertyChanged
  * Clicked
  * SelectedIndexChanged

### CS
* Атрибуты
* Конструкторы
* События окна/страницы
* События элементов
* Методы

### CSS
* display
* flex-direction
* align-items/justify-content/text-align
* gap

* position
* left/right/top/bottom
* transform
* margin/padding (max/min)

* height
* width

* overflow
* background-color
* border/border-bottom
* border-radius
* outline
* font-family
* font-size
* color
* cursor

### CSHML
* id
* class
* style
* type
* autocomplete
* placeholder
* src
* asp-action
* asp-controller
* visibility
* checked

## Развёртывание

### Создание новой базы
  - Делаем миграцию *add-migration Init -context ApplicationContext*
  - Обновляем базу *update-database -context ApplicationContext*