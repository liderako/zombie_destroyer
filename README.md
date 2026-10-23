# Zombie Destroyer

Core-механика гиперказуальной игры: машина едет вперёд по дороге, игрок управляет турелью на крыше.
Зомби стоят и бродят вдоль дороги, при приближении машины бегут к ней и бьют. Нужно доехать до финиша
и не потерять всё HP машины.

## Запуск

- **Unity 6000.3.11f1**, URP. Платформа — Android, портретная ориентация.
- Открыть `Assets/Game/Scenes/SplashScreen.unity` и нажать Play.

| Действие | Ввод |
|---|---|
| Старт уровня | тап по экрану |
| Прицел | палец указывает точку на экране — турель поворачивается и стреляет туда. Стрельба автоматическая |
| Рестарт / следующий уровень | тап по экрану «You lose» / «You win» |

## Игровой поток

```
SplashScreen → GameplayScene: машина стоит, «Tap to start»
тап                → машина разгоняется, камера следует, турель стреляет
HP машины = 0      → OnFailGame → «You lose» → тап → уровень заново
финиш              → OnFinish → машина проезжает финиш, экран затухает → «You win» → тап → следующий уровень
```

Поток построен на событиях `ProgressTrackerComponent` шаблона: победа и поражение — два события и два
независимых слушателя (`WinScreenPresenter` и `LoseUI`). Рестарт перезагружает сцену через `LevelManager`.

## Код

Код игры — `Assets/Game/Scripts/Entities`, UI — `Assets/Game/Scripts/GameUI`, конфиги — `HcCoreConfigs`.
DI — VContainer (`ProjectContext` / `SceneContext` / `GameObjectContext` шаблона), асинхронность — UniTask.

```
Entities/
├── Car/            CarComponent (HP, тряска камеры), CarMovementComponent (разгон, лёгкое покачивание,
│                   прогресс по метрам), CarWheelsComponent
├── Turret/         TurretAimComponent (поворот к точке под пальцем), TurretGunComponent (автострельба,
│                   пул пуль), ProjectileComponent (SphereCast, trail)
├── Zombies/        ZombieComponent (HP, агрессия, атака, смерть), ZombieMovementComponent,
│   │               ZombieViewComponent (Animator)
│   ├── Ai/         GOAP: ключи, цель, действия, сенсоры, фабрика типа агента
│   └── Editor/     StickmanRigBuilder — генератор рига для stickman.fbx
├── Combat/         Health, IDamageable
├── Blackboards/    ZombiesBlackboard — зомби уровня, счётчик убийств, события урона
├── Constructor/    LevelInitializer, GroundInitializer, ZombiesInitializer
└── PlayerInput/    PlayerGameplayInputListener
GameUI/             HealthBarUI, LevelProgressUI, KillCounterUI, DamagePopupsUI, ZombieHealthBarUI, ...
```

### Зомби на GOAP (CrashKonijn GOAP 3.1.2)

```
AttackCarGoal
 └── AttackCarAction   условие Aggroed, цель — машина, PerformWhileMoving
      └── WanderAction  цель — точка блуждания у места спавна, MoveBeforePerforming
```

- Пока зомби не агрессивен, `WanderAction` водит его шагом между точками слева и справа от места спавна
  с паузами 2–5 с. Старт у каждого свой, поэтому толпа не ходит в такт.
- Сенсор `Aggroed` = машина ближе 26.4 м **или** в зомби попала пуля. `ProactiveControllerBehaviour`
  перепланирует каждые 0.25 с и переключает зомби на атаку.
- GOAP решает, что делать, а тело двигает `ZombieMovementComponent`. Зомби бежит в точку перехвата
  машины со своим смещением вдоль борта и покачиванием. Скорость у каждого своя (±20%).
  Атакует только спереди или сбоку, чтобы турели не приходилось стрелять в камеру.

### Пулы

- Пули — `UnityEngine.Pool.ObjectPool` в `TurretGunComponent`, с прогревом.
- Всплывающие цифры урона — такой же пул в `DamagePopupsUI`.
- Зомби создаются один раз на уровень, а рестарт перезагружает сцену, поэтому пул для них не нужен.

### Анимации зомби

У `stickman.fbx` нет скелета, поэтому риг для него строит `Tools > Zombie Destroyer > Rebuild Stickman Rig`
(`StickmanRigBuilder`):

1. скелет из 21 гуманоидной кости по пропорциям меша;
2. скиннинг вершин к костям своей части тела;
3. Humanoid Avatar;
4. пересборка `View` в префабе `Zombie`.

Клипы — из пака Nephilite «Zombie Animation Set» (In Place), ретаргет через Humanoid.

`AC_Zombie`:

| Слой | Состояния |
|---|---|
| Base | blend tree idle / walk / run по скорости зомби, смерть по триггеру |
| Attack | удар по триггеру, маска верхней части тела — ноги продолжают бежать |

## Сцена и ассеты

- **Камера:** `CinemachineCamera` сзади-сверху в мировых осях, импульс при ударах по машине.
- **Свет:** один Directional Light с мягкими тенями и линейный туман на конце дороги, без запекания.
- **Физика:** слои `Zombie`, `Ground`, `Corpse`. Пули — `SphereCast` без Rigidbody. Убитый зомби становится
  динамическим Rigidbody и получает импульс от пули.
- **Эффекты:**
  - trail у пуль;
  - следы шин (TrailRenderer);
  - дым выхлопа;
  - вспышка выстрела и частицы попадания;
  - всплывающие цифры урона;
  - полоски HP над ранеными зомби.
- **Префабы:** `Assets/Game/Prefabs/Entities`. Материалы — `Graphics/Materials`, эффекты — `VFX`, модели — `Assets/Art/Models`.

## Баланс

Все числа — в ScriptableObject-конфигах, код менять не нужно:

| Конфиг | Значения |
|---|---|
| `Configs/Levels/Level_01…03` | длина 260 / 300 / 340 м, зомби 38 / 50 / 63 |
| `Configs/Settings/Gameplay/CarSettings` | HP 100, 13 м/с, после финиша проезжает 25 м и тормозит |
| `Configs/Settings/Gameplay/TurretSettings` | 7 выстр/с, урон 25, пуля 45 м/с, дальность ~160 м |
| `Configs/Settings/Gameplay/ZombieSettings` | HP 50, бег 6.98 м/с ±20%, агрессия 26.4 м, удар 6 раз в 1 с |

## Что исправлено в шаблоне

- `VContainerSettings` добавлен в Preloaded Assets — без него корневой `ProjectContext` не создавался.
- Добавлен `ProgressTrackerInstaller` и событие `ProgressTrackerComponent.OnStartGame` (старт по тапу).
- `AnalyticsComponentInstaller` регистрировал обёртку `Ref<>` вместо реализации; добавлена заглушка `AnalyticsComponent`.
- В `[UI].prefab` восстановлены пустые ссылки `WinUI`, `CoinBarUI`, `SettingsUI`, `CounterUI`.
  Удалены компоненты с отсутствующими скриптами.
- Включён URP (`Mobile_RPAsset`), импортированы TMP Essential Resources.
- Твины `UIBlink`, `ScaleUI`, `FadeUI`, `TranslateUI`, `RectLerpUI`, `CounterUI` не останавливались при
  уничтожении объекта и падали в безопасный режим DOTween после перезагрузки сцены.
  Теперь они привязаны к объекту (`SetLink`), а `UIBlink` останавливает `Sequence` через `Kill()`.
- `using UnityEditor` в рантайм-коде (`CoreEngine`, `CoreGrid`) обёрнут в `#if UNITY_EDITOR`,
  иначе не собирался билд под устройство.

## Ограничения

- Клип бега рассчитан на 3.2 м/с, зомби бегут быстрее. Клип ускоряется не больше чем в 2.2 раза,
  поэтому на полной скорости ноги немного скользят.
- Окно настроек шаблона не ставит игру на паузу.
