# auth_project/urls.py
from django.contrib import admin
from django.urls import path
from django.contrib.auth import views as auth_views
from accounts import views as accounts_views
from accounts.views import SignUpView

urlpatterns = [
    path('', auth_views.LoginView.as_view(template_name='registration/login.html'), name='login'),
    path('home/', accounts_views.home, name='home'),  # 确保这一行存在且正确
    path('signup/', SignUpView.as_view(), name='signup'),
   path('logout/', auth_views.LogoutView.as_view(next_page='/'), name='logout'),
    # 添加下面这行新路径
    path('delete_account/', accounts_views.delete_account, name='delete_account'),


    path('admin/', admin.site.urls),
]