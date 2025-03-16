using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace contact_list_management
{
    public partial class Form1 : Form
    {
        private List<Contact> contacts = new List<Contact>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFirstName.Text) &&
                !string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                var contact = new Contact
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text,
                    Category = cmbCategory.Text
                };
                contacts.Add(contact);
                UpdateContactList();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Введіть хоча б ім'я та телефон.", "Помилка");
            }
        }

        private void UpdateContactList()
        {
            listBoxContacts.DataSource = null;
            listBoxContacts.DataSource = contacts;
            listBoxContacts.DisplayMember = "DisplayInfo";
        }

        private void ClearInputFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            cmbCategory.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchTerm = txtSearch.Text.ToLower();
            var filteredContacts = contacts.Where(c => c.FirstName.ToLower().Contains(searchTerm)
                                                    || c.Phone.Contains(searchTerm)).ToList();
            listBoxContacts.DataSource = null;
            listBoxContacts.DataSource = filteredContacts;
            listBoxContacts.DisplayMember = "DisplayInfo";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var category = cmbCategoryFilter.Text;
            var filteredContacts = contacts.Where(c => c.Category == category).ToList();
            listBoxContacts.DataSource = null;
            listBoxContacts.DataSource = filteredContacts;
            listBoxContacts.DisplayMember = "DisplayInfo";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxContacts.SelectedItem is Contact selectedContact)
            {
                contacts.Remove(selectedContact);
                UpdateContactList();
            }
            else
            {
                MessageBox.Show("Виберіть контакт для видалення.", "Помилка");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var writer = new StreamWriter("contacts.txt"))
            {
                foreach (var contact in contacts)
                {
                    writer.WriteLine($"{contact.FirstName},{contact.LastName},{contact.Phone},{contact.Email},{contact.Category}");
                }
            }
            MessageBox.Show("Контакти успішно збережено.", "Успіх");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists("contacts.txt"))
            {
                contacts.Clear();
                var lines = File.ReadAllLines("contacts.txt");
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    var contact = new Contact
                    {
                        FirstName = data[0],
                        LastName = data[1],
                        Phone = data[2],
                        Email = data[3],
                        Category = data[4]
                    };
                    contacts.Add(contact);
                }
                UpdateContactList();
                MessageBox.Show("Контакти успішно завантажено.", "Успіх");
            }
            else
            {
                MessageBox.Show("Файл із контактами не знайдено.", "Помилка");
            }
        }
    }

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string DisplayInfo => $"{FirstName} {LastName} - {Phone}";
    }
}
