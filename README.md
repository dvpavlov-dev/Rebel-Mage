# Rebel Mage
![Gameplay](https://github.com/dvpavlov-dev/Rebel-Mage/blob/master/Assets%20for%20readme/Gameplay.gif)

## Описание

**Rebel Mage** — это изометрическая игра, разработанная на Unity версии 2022.3.57. 

В игре реализована система заклинаний с тремя доступными заклинаниями (отталкивающего и замедляющего типов) и два типа врагов с различными вариантами спавна. Игрок сможет пройти три уровня, после чего сложность увеличится, и количество врагов удвоится.

## Начальная точка

- **Сцена Start**: предназначена для инициализации глобальных сервисов, находящихся в ProjectContext, и перехода на сцену Gameplay с помощью `LoadingSceneService`.
  
- **Game.cs**: это начальная точка на сцене Gameplay, которая запускается после загрузки сцены через `LoadingSceneService`.

## Используемые плагины

- **Zenject**: используется для хранения конфигураций, префабов, списка заклинаний и прогресса игры.
  
- **R3 (UniRx)**: позволяет отслеживать прогресс загрузки сцены и постепенно загружать 200 префабов врагов в пул при переходе на сцену Gameplay.
  
- **DoTween 2.0**: применяется для анимации интерфейса пользователя (UI).

## Ресурсы

В этом проекте использованы модели и анимации из [Mixamo](https://www.mixamo.com/), а также ассеты из [Asset Store](https://assetstore.unity.com/) и материалы из открытых источников.
