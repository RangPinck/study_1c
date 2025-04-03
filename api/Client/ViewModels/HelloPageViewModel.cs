using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Client.ViewModels
{
	public class HelloPageViewModel : ViewModelBase
	{
		public HelloPageViewModel()
		{
			_bodyText = strings[_count];
		}
		private string _buttonText = "Далее";
        string _bodyText = "";
		int _count = 0;
		string[] strings = { "Перед тобой документ - план твоего развития на ближайшие три месяца. \r\nВпереди тебя ждет очень много информации. " +
                "\r\nТебе нужно узнать, что такое 1С, научиться работать на уровне покруче профессиональных пользователей программных продуктов 1С.  :)\r\n" +
                "Это - твоя цель на ближайшее время. Для того, чтобы мы тебя не потеряли под информационной лавиной, и создана эта программа.\r\nНе переживай: упорство и труд - всё перетрут :-)","Мы живем по простым принципам. Присоединяйся и ты: \r\n " +
                "0. Мозг включаем по умолчанию. \n 1. Задаем вопросы. В первую очередь себе после изучения материала, после любому коллеге/своему руководителю/ если что-то не понятно. \n 2. Ошибки - это нормально. “Попробовал - не получилось”, “придумал - не прокатило” - чем больше ты пробуешь, тем больше получится. Нужно продолжать делать, продолжать принимать решения. Ошибок не будет только если вообще ничего нового не делать. " +
				"\n\n\n Обратная связь - наше всё! Помоги стать лучше!\n\n" +
				"Если есть предложения, комментарии, замечания,особенно если какие-то материалы оказались нужными, но их не было в этом списке, и ты их получил от коллег/запрашивал через кординатора/скачивал из сети - напиши преподавателю."};

        public string BodyText { get => _bodyText; set => this.RaiseAndSetIfChanged(ref _bodyText, value); }
        public string ButtonText { get => _buttonText; set => this.RaiseAndSetIfChanged(ref _buttonText, value); }

        public void SkipAll()
		{
            MainWindowViewModel.Instance.PageContent = new CoursePage();
        }
		public void Next()
		{
			if (_count == strings.Length - 2)
			{
			ButtonText = "На главную";
            }
			if (_count == strings.Length-1) {
				MainWindowViewModel.Instance.PageContent = new CoursePage();
			}
			else
			{
				_count++;
				BodyText = strings[_count];
            }

        }
	}
}