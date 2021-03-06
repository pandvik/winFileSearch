# winFileSearch

Тестовое задание
Тема: Программа для поиска файлов по заданным критериям
Входные данные
У пользователя должна быть возможность указать перед началом поиска следующие параметры:
1. Стартовая директория (с которой начинается поиск)
2. Шаблон имени файла
3. Текст, содержащийся в файле, который необходимо искать
Требования к реализации
1. Введенные критерии поиска не должны потеряться при перезапуске программы.
2. Во время поиска нужно отображать прогресс, т.е. какой файл обрабатывается в данный 
момент, количество обработанных файлов и прошедшее время. 
o [Не обязательно] Плюсом будет, если программа также будет отображать 
расчетное время до завершения поиска.
3. Все найденные файлы должны отображаться в виде дерева (как в левой части 
проводника)
4. Данные в интерфейсе программы должны обновляться в реальном времени, программа 
не должна зависать.
5. Должна быть возможность остановить процесс поиска в любой момент.
Программа должна быть реализована на Windows Forms, предпочтительно с использованием 
среды Visual Studio 2013/2015
Плюсом буде, если код программы должен быть выложен на GitHub.


Описание структуры программы:

Решение состоит из трёх частей:
  winFileSearchLib - Классы, отвечающие за обход дерева каталогов и поиск по файлу
  winFileSearchApp - Графический интерфейс программы
    Form.cs - Собственно форма приложения и обработчики событий
    Form_helpers.cs - Вспомогательные методы, относящиеся к обновлению графического интерфейса
  winFileSearchTests - Тесты для winFileSearchLib
  
Алгоритмы: 
  Поиск по файлу реализован с помощью алгоритма Бойера — Мура (class FileSearch). 
  В нём используется Циклическая очередь (class CircularQueue)

Файл настроек храниться по адресу <%ApplicationData%>/FileSearchApp.xml.
Название файла можно изменить Resources.
За загрузку и сохранение данных из полей отвечает class SessionData.

Проблемы:
  - При очень большой скорости (~ >500 файлов/сек) добавления файлов в дерево файлов возникают фризы (~700мс). 
    Особенно это может быть заметно, если ОС уже подгрузила файлы в оперативную память.
  - Строка с обрабатываемым (в данный момент) файлом обновляется каждые 100 мс => не все файлы там будут отображаться.



