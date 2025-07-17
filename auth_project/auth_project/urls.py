# auth_project/urls.py
from django.contrib import admin
from django.urls import path
from django.contrib.auth import views as auth_views
from accounts import views as accounts_views
from accounts import views as accounts_views
from django.urls import path, include # 确保导入了 include

urlpatterns = [
    path('', auth_views.LoginView.as_view(template_name='registration/login.html'), name='login'),
    path('home/', accounts_views.home, name='home'),  # 确保这一行存在且正确
    # Correct code
    path('signup/', accounts_views.signup, name='signup'),
    path('logout/', auth_views.LogoutView.as_view(next_page='/'), name='logout'),
    # 添加下面这行新路径
    path('delete_account/', accounts_views.delete_account, name='delete_account'),
    # 添加下面这行激活 URL
    path('activate/<slug:uidb64>/<slug:token>/', accounts_views.activate, name='activate'),



    path('admin/', admin.site.urls),
    # 添加下面这行，为验证码应用提供URL
    path('captcha/', include('captcha.urls')),
]