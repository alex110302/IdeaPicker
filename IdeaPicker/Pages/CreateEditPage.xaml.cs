using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdeaPicker.Models;

namespace IdeaPicker.Pages;

public partial class CreateEditPage : ContentPage
{ 
    private ObservableCollection<Idea> _ideas = new ObservableCollection<Idea>();
    
    private List<Idea> _toBeDeletedIdeas = new List<Idea>();
    private List<Idea> _toBeAddedIdeas = new List<Idea>();
    private List<Idea> _toBeUpdatedIdeas = new List<Idea>();
    
    private int _topicId;
    
    private string _topicName;
    
    public CreateEditPage(string topicName, int topicId)
    {
        InitializeComponent();
        Title = "Create A List";
        _topicId = topicId;
        _topicName = topicName;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        txtTopicName.Text = _topicName;

        SetIdeasList();
        
        var ideaTemplate = new DataTemplate(typeof(TextCell));
        ideaTemplate.SetBinding(TextCell.TextProperty, "Description");

        lstIdeas.ItemTemplate = ideaTemplate;
        lstIdeas.ItemsSource = _ideas;
    }
    
    private void SetIdeasList()
    {
        _ideas.Clear(); 
        foreach (Idea idea in Repository.GetSpecificIdeaList(_topicId)) _ideas.Add(idea); 
    }

    private void BtnSaveTopicName_OnClicked(object sender, EventArgs e)
    {
        _topicName = txtTopicName.Text;
    }

    private void BtnSaveIdeaDescription_OnClicked(object sender, EventArgs e)
    {
        Idea idea = lstIdeas.SelectedItem as Idea;
        if (idea != null)
        {
             /*********************************************
             * For some reason when changing properties   *
             * for the idea that is slected in the        *
             * List view it also changes the values       *
             * in the corresponding idea in cahsedideas.  *
             * So I had to save the value of the          *
             * Description, remove the idea and then add  *
             * the idea back into chased ideas.           *
             * Basically when I change something in one   *
             * List it will change it in another even     *
             * after it as been added to the other list...*
             *********************************************/
            string tempDescription = idea.Description;
            Repository.CashedIdeas.Remove(idea);
            
            _ideas.Remove(idea);
            idea.Description = txtIdea.Text;
            
            _ideas.Add(new Idea
            {
                Id = idea.Id,
                Description = idea.Description,
                TopicId = idea.TopicId
            });
            _toBeUpdatedIdeas.Add(new Idea
            {
                Id = idea.Id,
                Description = idea.Description,
                TopicId = idea.TopicId
            });
            
            idea.Description = tempDescription;
            Repository.CashedIdeas.Add(idea);
        }
        else DisplayAlert("No Idea Selected", "Please select an idea to edit!", "Ok");
    }
    
    private void BtnUpdate_OnClicked(object sender, EventArgs e)
    {
        List<Idea> test = _toBeUpdatedIdeas;
        test = test;
        if (_toBeUpdatedIdeas.Count > 0) foreach (Idea idea in _toBeUpdatedIdeas) Repository.UpdateIdea(idea);
        if (_toBeDeletedIdeas.Count > 0) foreach (Idea idea in _toBeDeletedIdeas) Repository.DeleteIdea(idea);
        if (_toBeAddedIdeas.Count > 0)
            foreach (Idea idea in _toBeAddedIdeas)
            {
                Repository.SetIdea(idea);
                Repository.UpdateIdea(idea);
            }
        if (_toBeAddedIdeas.Count > 0 || _toBeDeletedIdeas.Count > 0 || _toBeUpdatedIdeas.Count > 0)
        {
            _toBeAddedIdeas.Clear();
            _toBeDeletedIdeas.Clear();
            _toBeUpdatedIdeas.Clear();
        }
        
        Topic topic = Repository.GetSpecificTopic(_topicId);
        topic.Name = _topicName;
        Repository.UpdateTopic(topic);
    }
    
    private void BtnAdd_OnClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtIdea.Text))
        {
            Idea idea = new Idea
            {
                Description = txtIdea.Text,
                TopicId = _topicId
            };
            
            _toBeAddedIdeas.Add(idea);
            _ideas.Add(idea);
            
            txtIdea.Text = string.Empty;
        }
        else DisplayAlert("No Idea Description", "Please Describe an idea to Add!", "Ok");
    }
    
    private void BtnDelete_OnClicked(object sender, EventArgs e)
    {
        Idea idea = lstIdeas.SelectedItem as Idea;
        if (idea != null)
        {
            txtIdea.Text = string.Empty;
            
            _toBeDeletedIdeas.Add(idea);
            _ideas.Remove(idea);

            lstIdeas.SelectedItem = null;
        }
        else DisplayAlert("No Idea Selected", "Please select and Idea to delete!", "Ok");
    }

    private void LstIdeas_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Idea idea = lstIdeas.SelectedItem as Idea;
        if (idea != null) txtIdea.Text = idea.Description;
    }
}