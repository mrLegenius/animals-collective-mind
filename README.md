# Моделирование коллективного разума животных в играх

Данная работа нацелена на моделирование коллективного поведения животных. В частности реализована симуляция прайда львов в естественной среде.
Основная часть искусственного интеллекта - принятие решения, основана на подходе Utility-based AI.  
Проект реализован на движке Unity полностью на языке C#

Проект является частью магистерской выпускной квалификационной работы по образовательной программе "Технологии разработки компьютерных игр" [Школы разработки видеоигр Университета ИТМО](https://itmo.games/).

## Обзор

Система искусственного интелекта состоит из 4 подсистем: Восприятие, Память, Коммуникация и Принятие решений.

### Восприятие
Каждый агент может обладать несколькими сенсорами. 
Сейчас поддержано 3 типа сенсоров: Зрения, слуха, страха.

Поиск пути реализован с помощью пакета AI Navigation от Unity Technologies Inc. 

### Память

Хранит факты об определенном объекте в виде стимулов (факт + приоритет). 
Со временем стимул теряет приоритет и когда он опустится до порогового значения, он удалиться из памяти. 

### Принятие решений

Основано на Unity-based AI. 
Каждый агент обладает набором поведений, которые в свою очередь состоят из:
- Действия
- Набора функций оценки 
- Комбинатора ошибок
- Генератора контекста

### Коммуникация

Агенты могут создавать и присоединяться в группы, чтобы достичь единой цели. В группе они могут передавать информацию друг другу.

### Элементы прототипа

В симуляции участвуют львы и антилопы. Также в мире присутствуют высокая трава, источники воды и пищи.
 
![image](https://github.com/mrLegenius/animals-collective-mind/assets/22073131/b62f3483-dafd-49c3-befc-15089a8dfbc4)


## Требования
Для запуска проекта потребуется Unity 2022.3 или выше.

## Использование
В проекте находится 3 рабочих сцены в директории **Assets/Lions/**: **Scenario 1**, **Scenario 2**, **Scenario 3**.

Каждое животные имеет свои характеристики:

![image](https://github.com/mrLegenius/animals-collective-mind/assets/22073131/79966586-4e31-43b1-adc7-04677e4e3e41)

И набор настраиваемых сенсоров:

![image](https://github.com/mrLegenius/animals-collective-mind/assets/22073131/56181087-79e1-4100-9070-ae7305ed6f48)


Каждый регион состоит из определенного числа высокой травы и источников воды

![image](https://github.com/mrLegenius/animals-collective-mind/assets/22073131/cc9af5b3-e54b-4457-9f66-7ea378765c7a)

## Дополнительно

Работу выполнил: Лебедев Евгений

Текст ВКР доступен по [ссылке](https://github.com/mrLegenius/animals-collective-mind/blob/main/%D0%A2%D0%B5%D0%BA%D1%81%D1%82_%D0%92%D0%9A%D0%A0.pdf)

 

