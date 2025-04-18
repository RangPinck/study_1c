# 1C Study

Программа для обучения стажёров, сбора статистики и управление процессом обучения программе 1С и не только.

## Особенности

- Полная система авторизации под управлением администраторов
- Ведение статистики по каждому пользователю
- Создание, редактирование и удаление курсов.

## Технологии 

- Avalonia UI - Фреймворк для кроссплатформенной клиентской разработки
- RestAPI – архитектурный подход для создания API
- EntityFramework – средство сопоставления отношений объектов
- PostgreSQL – СУБД
- Docker – программное обеспечение для автоматизации развёртывания и управления приложениями в средах с поддержкой контейнеризации
- ASP.NET Core – Фреймворк для создания web-приложений и web-api на платформе .Net

## Необходимые условия

Что нужно для установки программного обеспечения и как его установить

    Необходимая операционная система:
        Windows 7 и выше
        macOS 10.13 (High Sierra)
        Linux

    Процессор:
        Минимально: 2 физических, 4 виртуальных ядра и 2 ГГц
        Оптимально: 4 физических ядра или больше и 2.6 ГГц или выше

    Оперативная память:
        Минимально: 4 гигабайта
        Оптимально: 8 гигабайта и больше

    Место на диске:
        Минимально: 2 гигабайта свободного пространства
        Оптимально: 8 гигабайт свободного пространства и больше


## Начало работы

   1. Cклонировать репозиторий или скачать и распаковать архив с программой;
   2. Скачать Docker контейнер по выданной ссылке;
   3. Открыть решение в IDE;
   4. Изменить строку подключения к базе данных в классе TradeDbContext.cs на ту, что использовалась при подключении к базе данных, созданной ранее.
   5. Запустить проект.     
  
## Авторы 
 Чернов С.В. – Backend, Database, Testing –  https://github.com/RangPinck
 Горбачёв В.А. – Frontend, Database, Testing – https://github.com/Howaitoniksu
