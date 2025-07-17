# accounts/views.py
from django.shortcuts import render, redirect
from django.urls import reverse_lazy
from django.views import generic
from .forms import CustomUserCreationForm
from django.contrib.auth import logout
from django.contrib import messages
from django.contrib.auth.decorators import login_required
from django.shortcuts import render, redirect
from django.contrib.auth import login
from django.contrib.sites.shortcuts import get_current_site
from django.utils.http import urlsafe_base64_encode, urlsafe_base64_decode
from django.utils.encoding import force_bytes, force_str
from django.template.loader import render_to_string
from django.core.mail import EmailMessage
from django.contrib.auth.tokens import default_token_generator
from .models import CustomUser # 确保导入了你的用户模型
from .forms import CustomUserCreationForm

# 用下面的函数视图替换掉 SignUpView 类
def signup(request):
    if request.method == 'POST':
        form = CustomUserCreationForm(request.POST)
        if form.is_valid():
            # 创建用户，但暂时不保存到数据库
            user = form.save(commit=False)
            # 将用户设置为未激活状态
            user.is_active = False
            user.save()

            # 发送验证邮件
            current_site = get_current_site(request)
            mail_subject = '激活您的账户'
            message = render_to_string('registration/account_activation_email.html', {
                'user': user,
                'domain': current_site.domain,
                'uid': urlsafe_base64_encode(force_bytes(user.pk)),
                'token': default_token_generator.make_token(user),
            })
            to_email = form.cleaned_data.get('email')
            email = EmailMessage(
                mail_subject, message, to=[to_email]
            )
            email.send()

            return render(request, 'registration/account_activation_sent.html')
    else:
        form = CustomUserCreationForm()
    return render(request, 'registration/signup.html', {'form': form})


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


def activate(request, uidb64, token):
    try:
        uid = force_str(urlsafe_base64_decode(uidb64))
        user = CustomUser.objects.get(pk=uid)
    except(TypeError, ValueError, OverflowError, CustomUser.DoesNotExist):
        user = None

    if user is not None and default_token_generator.check_token(user, token):
        user.is_active = True
        user.save()
        login(request, user)
        messages.success(request, '恭喜您！您的账户已成功激活。')
        return redirect('home')
    else:
        return render(request, 'registration/account_activation_invalid.html')