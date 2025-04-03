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
		private string _buttonText = "�����";
        string _bodyText = "";
		int _count = 0;
		string[] strings = { "����� ����� �������� - ���� ������ �������� �� ��������� ��� ������. \r\n������� ���� ���� ����� ����� ����������. " +
                "\r\n���� ����� ������, ��� ����� 1�, ��������� �������� �� ������ ������� ���������������� ������������� ����������� ��������� 1�.  :)\r\n" +
                "��� - ���� ���� �� ��������� �����. ��� ����, ����� �� ���� �� �������� ��� �������������� �������, � ������� ��� ���������.\r\n�� ���������: �������� � ���� - �� �������� :-)","�� ����� �� ������� ���������. ������������� � ��: \r\n " +
                "0. ���� �������� �� ���������. \n 1. ������ �������. � ������ ������� ���� ����� �������� ���������, ����� ������ �������/������ ������������/ ���� ���-�� �� �������. \n 2. ������ - ��� ���������. ����������� - �� �����������, ��������� - �� ��������� - ��� ������ �� ��������, ��� ������ ���������. ����� ���������� ������, ���������� ��������� �������. ������ �� ����� ������ ���� ������ ������ ������ �� ������. " +
				"\n\n\n �������� ����� - ���� ��! ������ ����� �����!\n\n" +
				"���� ���� �����������, �����������, ���������,�������� ���� �����-�� ��������� ��������� �������, �� �� �� ���� � ���� ������, � �� �� ������� �� ������/���������� ����� �����������/�������� �� ���� - ������ �������������."};

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
			ButtonText = "�� �������";
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