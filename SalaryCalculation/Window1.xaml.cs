using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using SalaryCalculation.DBmodel;


namespace SalaryCalculation
{
	/// <summary>
	/// Логика взаимодействия для Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{

		public SalaryEntities entObj = new SalaryEntities();
		public Window1()
		{
			InitializeComponent();

			ComboBox_Position.SelectedValuePath = "Id";

			ComboBox_Position.DisplayMemberPath = "Name";

			ComboBox_Position.ItemsSource = entObj.Position.ToList();

			ComboBox_Experience.SelectedValuePath = "Id";

			ComboBox_Experience.DisplayMemberPath = "Name";

			ComboBox_Experience.ItemsSource = entObj.Experience.ToList();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			// Получение данных из текстовых полей
			string fio = TextBox_FIO.Text;
			string id = TextBox_ID.Text;

			// Извлечение выбранных значений из комбобоксов
			var selectedPosition = ComboBox_Position.SelectedItem as Position; // Предполагается, что Position - это класс
			var selectedExperience = ComboBox_Experience.SelectedItem as Experience; // Предполагается, что Experience - это класс

			string position = selectedPosition?.Name; // Предполагается, что у Position есть свойство Name
			string experience = selectedExperience?.Name; // Предполагается, что у Experience есть свойство Name

			// Проверка на пустые поля
			if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(id) ||
				string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(experience) ||
				string.IsNullOrWhiteSpace(TextBox_HoursWorked.Text) ||
				string.IsNullOrWhiteSpace(TextBox_ModelsDesigned.Text) ||
				string.IsNullOrWhiteSpace(TextBox_LateCount.Text) ||
				string.IsNullOrWhiteSpace(TextBox_AbsentCount.Text) ||
				string.IsNullOrWhiteSpace(TextBox_DefectiveModels.Text) ||
				string.IsNullOrWhiteSpace(TextBox_PhoneUsageCount.Text) ||
				string.IsNullOrWhiteSpace(TextBox_EarlyLeaveCount.Text))
			{
				MessageBox.Show("Пожалуйста, заполните все поля.");
				return;
			}

			// Проверка на наличие цифр в ФИО
			if (!fio.All(char.IsLetter))
			{
				MessageBox.Show("ФИО должно содержать только буквы.");
				return;
			}

			// Проверка на ввод чисел в текстовых полях
			if (!int.TryParse(TextBox_HoursWorked.Text, out int hoursWorked) ||
				!int.TryParse(TextBox_ModelsDesigned.Text, out int modelsDesigned) ||
				!int.TryParse(TextBox_LateCount.Text, out int lateCount) ||
				!int.TryParse(TextBox_AbsentCount.Text, out int absentCount) ||
				!int.TryParse(TextBox_DefectiveModels.Text, out int defectiveModels) ||
				!int.TryParse(TextBox_PhoneUsageCount.Text, out int phoneUsageCount) ||
				!int.TryParse(TextBox_EarlyLeaveCount.Text, out int earlyLeaveCount))
			{
				MessageBox.Show("Пожалуйста, введите корректные числовые значения в соответствующие поля.");
				return;
			}

			// Логика расчета зарплаты и премии
			double baseSalary = 50000; // Базовая зарплата
			double hourlyRate = baseSalary / 160; // Предполагаем, что 160 часов в месяц
			double salary = hoursWorked * hourlyRate;

			// Вычеты
			double penaltyForLates = lateCount * 1000; // Штраф за опоздание
			double penaltyForAbsents = absentCount * 5000; // Штраф за прогул
			double penaltyForDefectiveModels = defectiveModels * 2000; // Штраф за бракованные модели
			double penaltyForPhoneUsage = phoneUsageCount * 500; // Штраф за использование телефона
			double penaltyForEarlyLeave = earlyLeaveCount * 1000; // Штраф за уход раньше времени

			// Общие вычеты
			double totalDeductions = penaltyForLates + penaltyForAbsents + penaltyForDefectiveModels + penaltyForPhoneUsage + penaltyForEarlyLeave;

			// Премия
			double bonus = modelsDesigned * 1500; // Премия за модели

			// Итоговая зарплата
			double finalSalary = salary + bonus - totalDeductions;

			// Формирование сообщения о штрафах
			string penaltiesMessage = "Штрафы:\n";
			if (penaltyForLates > 0) penaltiesMessage += $"- За опоздание: {penaltyForLates:C}\n";
			if (penaltyForAbsents > 0) penaltiesMessage += $"- За прогул: {penaltyForAbsents:C}\n";
			if (penaltyForDefectiveModels > 0) penaltiesMessage += $"- За бракованные модели: {penaltyForDefectiveModels:C}\n";
			if (penaltyForPhoneUsage > 0) penaltiesMessage += $"- За использование телефона: {penaltyForPhoneUsage:C}\n";
			if (penaltyForEarlyLeave > 0) penaltiesMessage += $"- За уход раньше времени: {penaltyForEarlyLeave:C}\n";

			// Вывод результата
			MessageBox.Show($"ФИО: {fio}\nID: {id}\nДолжность: {position}\nСтаж работы: {experience}\nИтоговая зарплата: {finalSalary:C}\n\n{penaltiesMessage}");
		}
	}
}