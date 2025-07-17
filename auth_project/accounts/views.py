# accounts/views.py
from django.shortcuts import render, redirect
from django.urls import reverse_lazy
from django.views import generic
from .forms import CustomUserCreationForm
from django.contrib.auth import logout
from django.contrib import messages
from django.contrib.auth.decorators import login_required

class SignUpView(generic.CreateView):
    form_class = CustomUserCreationForm
    success_url = reverse_lazy('login')
    template_name = 'registration/signup.html'

@login_required
def home(request):
    return render(request, 'dashboard/home.html')

# 在文件末尾添加这个新视图
@login_required
def delete_account(request):
    if request.method == 'POST':
        user = request.user
        # 登出用户，清理 session
        logout(request)
        # 删除用户对象
        user.delete()
        # 添加一条成功消息，它将在下个页面显示
        messages.success(request, '您的账户已成功注销。')
        # 重定向到首页（登录页）
        return redirect('login')

    # 如果是 GET 请求，就显示确认页面
    return render(request, 'dashboard/delete_account_confirm.html')