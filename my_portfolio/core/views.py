# core/views.py
from django.views.generic import ListView
from .models import Project

class HomePageView(ListView):
    model = Project
    template_name = 'core/home.html'
    context_object_name = 'project_list'