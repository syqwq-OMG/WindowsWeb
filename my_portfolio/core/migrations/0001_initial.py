# Generated by Django 5.2.4 on 2025-07-14 15:02

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Project',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('title', models.CharField(max_length=100, verbose_name='项目标题')),
                ('description', models.TextField(verbose_name='项目描述')),
                ('image', models.ImageField(upload_to='project_images/', verbose_name='项目主图')),
                ('technologies', models.CharField(help_text='请用逗号分隔', max_length=200, verbose_name='使用的技术')),
                ('project_url', models.URLField(blank=True, null=True, verbose_name='项目链接')),
            ],
            options={
                'verbose_name': '我的项目',
                'verbose_name_plural': '我的项目',
                'ordering': ['-id'],
            },
        ),
    ]
