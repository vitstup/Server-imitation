// See https://aka.ms/new-console-template for more information
using SimpleGameServerImmitation.Scripts;
using SimpleGameServerImmitation.Scripts.Launcher;
using SimpleGameServerImmitation.Scripts.Mocks;

Database database = new MockDatabase();

Launcher launcher = new Launcher();

Client client = new Client();

// В идеале нужно использовать Dependency Injection или расмотреть другие подходы. Но что бы не тратить на это время, тут реализовано через своего рода singleton
SingletonsHandler.database = database;
SingletonsHandler.client = client;
SingletonsHandler.currentSession = new CurrentSessionHandler();

// Запуск симуляции серверов
ServersSimulation.CreateServers(database.serversDatabase.data.ToArray());

// Запуск лаунчера
launcher.ShowInterface();

// У меня есть прямые обращения из клиента/лаунчера к базе данных и серверу. Хотя понятно в реальном проекте этого бы небыло. 
// Был бы какой нибудь класс коннектор, который по api получал бы данные и отправлял комманды на сервер. База данных бы взаимодействовала только с сервером.
// Тут же посколько это будет фактически просто дуюлированием кода и написанием бесмысленных классов с большой тратой времени - это реализованно не было.