# Closer
Auto closes the window or press need button


Автоматически закрывает окна или нажимает нужные кнопки.
Выбор окон и кнопок задется в файле настроек который ищет возле бинарника.
Перед закрытием окна или нажатием кнопки делает скрин и сохраняет в указанную в настройках папку.
В файле Settings.txt после общих настроек по поряку указывается имя окна и кнопка.

Пример фйла настроек Settings.txt :
% Комментарии
%
% !comp! - заменяет на имя компа
% !title! - заменяет на имя окна
% !exit! - закрывает окно
% &Нет - если первая буква подчеркнута
% [n] - если в названии есть символ переноса строки
%

% Папка для сохранения скриншотов
C:\temp\!comp!\img

% Имя фалов скриншотов
!title!

% Интервал таймера закрывалки (в мс)
1000

% Дальше идут настройки закрывания окон

% Заголовок окна
7-Zip

% Название кнопки
ОК

Калькулятор
!exit!

Test
&Нет

Test
ОК
