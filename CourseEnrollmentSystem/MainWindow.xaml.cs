using System.Linq;
using System.Windows;
using CourseEnrollmentSystem.Data;
using CourseEnrollmentSystem.Models;

namespace CourseEnrollmentSystem
{
    public partial class MainWindow : Window
    {
        private AppDbContext? _context;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            LoadCourses();
        }
        // Read operation
        private void LoadCourses()
        {
            CoursesGrid.ItemsSource =
                _context!.Courses
                         .OrderBy(c => c.Id)
                         .ToList();
        }

        // Create operation
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var course = new Course
            {
                CourseName = txtCourseName.Text,
                Instructor = txtInstructor.Text,
                Credits = int.TryParse(txtCredits.Text, out var c) ? c : 0
            };

            _context!.Courses.Add(course);
            _context.SaveChanges();
            LoadCourses();
            ClearInputs();
        }

        // Update and Delete operations
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesGrid.SelectedItem is Course selected)
            {
                var entity = _context!.Courses
                                      .First(c => c.Id == selected.Id);

                entity.CourseName = txtCourseName.Text;
                entity.Instructor = txtInstructor.Text;
                entity.Credits = int.TryParse(txtCredits.Text, out var c) ? c : 0;

                _context.SaveChanges();
                LoadCourses();
            }
        }

        // Delete operation
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesGrid.SelectedItem is Course selected)
            {
                var entity = _context!.Courses
                                      .First(c => c.Id == selected.Id);

                _context.Courses.Remove(entity);
                _context.SaveChanges();
                LoadCourses();
                ClearInputs();
            }
        }
        // Selection changed event to populate input fields
        private void CoursesGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CoursesGrid.SelectedItem is Course selected)
            {
                txtCourseName.Text = selected.CourseName;
                txtInstructor.Text = selected.Instructor;
                txtCredits.Text = selected.Credits.ToString();
            }
        }
        // Helper method to clear input fields
        private void ClearInputs()
        {
            txtCourseName.Clear();
            txtInstructor.Clear();
            txtCredits.Clear();
        }
    }
}
