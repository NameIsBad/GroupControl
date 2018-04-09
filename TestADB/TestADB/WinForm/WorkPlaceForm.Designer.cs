namespace GroupControl.WinForm
{
    partial class WorkPlaceForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.微信ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.好友 = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯录加好友 = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯录定时加好友 = new System.Windows.Forms.ToolStripMenuItem();
            this.搜索加好友 = new System.Windows.Forms.ToolStripMenuItem();
            this.朋友圈 = new System.Windows.Forms.ToolStripMenuItem();
            this.手动发朋友圈 = new System.Windows.Forms.ToolStripMenuItem();
            this.定时发朋友圈 = new System.Windows.Forms.ToolStripMenuItem();
            this.群ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手动发群消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动发群消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粉丝 = new System.Windows.Forms.ToolStripMenuItem();
            this.查看粉丝数 = new System.Windows.Forms.ToolStripMenuItem();
            this.历史粉丝数 = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯里 = new System.Windows.Forms.ToolStripMenuItem();
            this.检测通讯录 = new System.Windows.Forms.ToolStripMenuItem();
            this.导入通讯录 = new System.Windows.Forms.ToolStripMenuItem();
            this.回收数据 = new System.Windows.Forms.ToolStripMenuItem();
            this.导入历史记录 = new System.Windows.Forms.ToolStripMenuItem();
            this.闲鱼ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.快手ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关注点赞评论ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消所有任务 = new System.Windows.Forms.ToolStripMenuItem();
            this.取消当前任务 = new System.Windows.Forms.ToolStripMenuItem();
            this.历史任务 = new System.Windows.Forms.ToolStripMenuItem();
            this.辅助功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.解屏 = new System.Windows.Forms.ToolStripMenuItem();
            this.锁屏 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量重启 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量关机 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量安装软件 = new System.Windows.Forms.ToolStripMenuItem();
            this.更改输入法 = new System.Windows.Forms.ToolStripMenuItem();
            this.按编号搜索 = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.微信ToolStripMenuItem,
            this.闲鱼ToolStripMenuItem,
            this.快手ToolStripMenuItem,
            this.取消任务ToolStripMenuItem,
            this.辅助功能ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1370, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 微信ToolStripMenuItem
            // 
            this.微信ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.好友,
            this.朋友圈,
            this.群ToolStripMenuItem,
            this.粉丝,
            this.通讯里});
            this.微信ToolStripMenuItem.Name = "微信ToolStripMenuItem";
            this.微信ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.微信ToolStripMenuItem.Text = "微信";
            // 
            // 好友
            // 
            this.好友.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通讯录加好友,
            this.通讯录定时加好友,
            this.搜索加好友});
            this.好友.Name = "好友";
            this.好友.Size = new System.Drawing.Size(112, 22);
            this.好友.Text = "好友";
            // 
            // 通讯录加好友
            // 
            this.通讯录加好友.Name = "通讯录加好友";
            this.通讯录加好友.Size = new System.Drawing.Size(172, 22);
            this.通讯录加好友.Text = "通讯录手动加好友";
            this.通讯录加好友.Click += new System.EventHandler(this.通讯录加好友ToolStripMenuItem_Click);
            // 
            // 通讯录定时加好友
            // 
            this.通讯录定时加好友.Name = "通讯录定时加好友";
            this.通讯录定时加好友.Size = new System.Drawing.Size(172, 22);
            this.通讯录定时加好友.Text = "通讯录定时加好友";
            this.通讯录定时加好友.Click += new System.EventHandler(this.通讯录定时加好友ToolStripMenuItem1_Click);
            // 
            // 搜索加好友
            // 
            this.搜索加好友.Name = "搜索加好友";
            this.搜索加好友.Size = new System.Drawing.Size(172, 22);
            this.搜索加好友.Text = "搜索加好友";
            this.搜索加好友.Click += new System.EventHandler(this.搜索加好友ToolStripMenuItem2_Click);
            // 
            // 朋友圈
            // 
            this.朋友圈.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.手动发朋友圈,
            this.定时发朋友圈});
            this.朋友圈.Name = "朋友圈";
            this.朋友圈.Size = new System.Drawing.Size(112, 22);
            this.朋友圈.Text = "朋友圈";
            // 
            // 手动发朋友圈
            // 
            this.手动发朋友圈.Name = "手动发朋友圈";
            this.手动发朋友圈.Size = new System.Drawing.Size(148, 22);
            this.手动发朋友圈.Text = "手动发朋友圈";
            this.手动发朋友圈.Click += new System.EventHandler(this.手动发朋友圈ToolStripMenuItem_Click);
            // 
            // 定时发朋友圈
            // 
            this.定时发朋友圈.Name = "定时发朋友圈";
            this.定时发朋友圈.Size = new System.Drawing.Size(148, 22);
            this.定时发朋友圈.Text = "定时发朋友圈";
            this.定时发朋友圈.Click += new System.EventHandler(this.定时发朋友圈ToolStripMenuItem_Click);
            // 
            // 群ToolStripMenuItem
            // 
            this.群ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.手动发群消息ToolStripMenuItem,
            this.自动发群消息ToolStripMenuItem});
            this.群ToolStripMenuItem.Name = "群ToolStripMenuItem";
            this.群ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.群ToolStripMenuItem.Text = "群";
            // 
            // 手动发群消息ToolStripMenuItem
            // 
            this.手动发群消息ToolStripMenuItem.Name = "手动发群消息ToolStripMenuItem";
            this.手动发群消息ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.手动发群消息ToolStripMenuItem.Text = "手动发群消息";
            this.手动发群消息ToolStripMenuItem.Click += new System.EventHandler(this.手动发群消息ToolStripMenuItem_Click);
            // 
            // 自动发群消息ToolStripMenuItem
            // 
            this.自动发群消息ToolStripMenuItem.Name = "自动发群消息ToolStripMenuItem";
            this.自动发群消息ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.自动发群消息ToolStripMenuItem.Text = "自动发群消息";
            this.自动发群消息ToolStripMenuItem.Click += new System.EventHandler(this.自动发群消息ToolStripMenuItem_Click);
            // 
            // 粉丝
            // 
            this.粉丝.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看粉丝数,
            this.历史粉丝数});
            this.粉丝.Name = "粉丝";
            this.粉丝.Size = new System.Drawing.Size(112, 22);
            this.粉丝.Text = "粉丝";
            // 
            // 查看粉丝数
            // 
            this.查看粉丝数.Name = "查看粉丝数";
            this.查看粉丝数.Size = new System.Drawing.Size(136, 22);
            this.查看粉丝数.Text = "查看粉丝数";
            this.查看粉丝数.Click += new System.EventHandler(this.查看粉丝数ToolStripMenuItem_Click);
            // 
            // 历史粉丝数
            // 
            this.历史粉丝数.Name = "历史粉丝数";
            this.历史粉丝数.Size = new System.Drawing.Size(136, 22);
            this.历史粉丝数.Text = "历史粉丝数";
            this.历史粉丝数.Click += new System.EventHandler(this.历史粉丝数ToolStripMenuItem_Click);
            // 
            // 通讯里
            // 
            this.通讯里.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检测通讯录,
            this.导入通讯录,
            this.回收数据,
            this.导入历史记录});
            this.通讯里.Name = "通讯里";
            this.通讯里.Size = new System.Drawing.Size(112, 22);
            this.通讯里.Text = "通讯里";
            // 
            // 检测通讯录
            // 
            this.检测通讯录.Name = "检测通讯录";
            this.检测通讯录.Size = new System.Drawing.Size(148, 22);
            this.检测通讯录.Text = "检测通讯录";
            this.检测通讯录.Click += new System.EventHandler(this.检测通讯录ToolStripMenuItem_Click);
            // 
            // 导入通讯录
            // 
            this.导入通讯录.Name = "导入通讯录";
            this.导入通讯录.Size = new System.Drawing.Size(148, 22);
            this.导入通讯录.Text = "导入通讯录";
            this.导入通讯录.Click += new System.EventHandler(this.导入通讯录ToolStripMenuItem_Click);
            // 
            // 回收数据
            // 
            this.回收数据.Name = "回收数据";
            this.回收数据.Size = new System.Drawing.Size(148, 22);
            this.回收数据.Text = "回收数据";
            this.回收数据.Click += new System.EventHandler(this.回收数据ToolStripMenuItem_Click);
            // 
            // 导入历史记录
            // 
            this.导入历史记录.Name = "导入历史记录";
            this.导入历史记录.Size = new System.Drawing.Size(148, 22);
            this.导入历史记录.Text = "导入历史记录";
            this.导入历史记录.Click += new System.EventHandler(this.导入历史记录ToolStripMenuItem_Click);
            // 
            // 闲鱼ToolStripMenuItem
            // 
            this.闲鱼ToolStripMenuItem.Name = "闲鱼ToolStripMenuItem";
            this.闲鱼ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.闲鱼ToolStripMenuItem.Text = "闲鱼";
            // 
            // 快手ToolStripMenuItem
            // 
            this.快手ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关注点赞评论ToolStripMenuItem});
            this.快手ToolStripMenuItem.Name = "快手ToolStripMenuItem";
            this.快手ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.快手ToolStripMenuItem.Text = "快手";
            // 
            // 关注点赞评论ToolStripMenuItem
            // 
            this.关注点赞评论ToolStripMenuItem.Name = "关注点赞评论ToolStripMenuItem";
            this.关注点赞评论ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.关注点赞评论ToolStripMenuItem.Text = "关注 点赞 评论";
            // 
            // 取消任务ToolStripMenuItem
            // 
            this.取消任务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.取消所有任务,
            this.取消当前任务,
            this.历史任务});
            this.取消任务ToolStripMenuItem.Name = "取消任务ToolStripMenuItem";
            this.取消任务ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.取消任务ToolStripMenuItem.Text = "任务";
            // 
            // 取消所有任务
            // 
            this.取消所有任务.Name = "取消所有任务";
            this.取消所有任务.Size = new System.Drawing.Size(148, 22);
            this.取消所有任务.Text = "取消所有任务";
            this.取消所有任务.Click += new System.EventHandler(this.取消所有任务ToolStripMenuItem_Click);
            // 
            // 取消当前任务
            // 
            this.取消当前任务.Name = "取消当前任务";
            this.取消当前任务.Size = new System.Drawing.Size(148, 22);
            this.取消当前任务.Text = "取消当前任务";
            this.取消当前任务.Click += new System.EventHandler(this.取消当前任务ToolStripMenuItem_Click);
            // 
            // 历史任务
            // 
            this.历史任务.Name = "历史任务";
            this.历史任务.Size = new System.Drawing.Size(148, 22);
            this.历史任务.Text = "历史任务列表";
            this.历史任务.Click += new System.EventHandler(this.历史任务ToolStripMenuItem_Click);
            // 
            // 辅助功能ToolStripMenuItem
            // 
            this.辅助功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.解屏,
            this.锁屏,
            this.批量重启,
            this.批量关机,
            this.批量安装软件,
            this.更改输入法,
            this.按编号搜索});
            this.辅助功能ToolStripMenuItem.Name = "辅助功能ToolStripMenuItem";
            this.辅助功能ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.辅助功能ToolStripMenuItem.Text = "辅助功能";
            // 
            // 解屏
            // 
            this.解屏.Name = "解屏";
            this.解屏.Size = new System.Drawing.Size(148, 22);
            this.解屏.Text = "解屏";
            this.解屏.Click += new System.EventHandler(this.解屏ToolStripMenuItem_Click);
            // 
            // 锁屏
            // 
            this.锁屏.Name = "锁屏";
            this.锁屏.Size = new System.Drawing.Size(148, 22);
            this.锁屏.Text = "锁屏";
            this.锁屏.Click += new System.EventHandler(this.锁屏ToolStripMenuItem_Click);
            // 
            // 批量重启
            // 
            this.批量重启.Name = "批量重启";
            this.批量重启.Size = new System.Drawing.Size(148, 22);
            this.批量重启.Text = "批量重启";
            this.批量重启.Click += new System.EventHandler(this.批量重启ToolStripMenuItem_Click);
            // 
            // 批量关机
            // 
            this.批量关机.Name = "批量关机";
            this.批量关机.Size = new System.Drawing.Size(148, 22);
            this.批量关机.Text = "批量关机";
            this.批量关机.Click += new System.EventHandler(this.批量关机ToolStripMenuItem_Click);
            // 
            // 批量安装软件
            // 
            this.批量安装软件.Name = "批量安装软件";
            this.批量安装软件.Size = new System.Drawing.Size(148, 22);
            this.批量安装软件.Tag = "2";
            this.批量安装软件.Text = "批量安装软件";
            this.批量安装软件.Click += new System.EventHandler(this.批量安装软件ToolStripMenuItem_Click);
            // 
            // 更改输入法
            // 
            this.更改输入法.Name = "更改输入法";
            this.更改输入法.Size = new System.Drawing.Size(148, 22);
            this.更改输入法.Tag = "1";
            this.更改输入法.Text = "更改输入法";
            this.更改输入法.Click += new System.EventHandler(this.更改输入法ToolStripMenuItem_Click);
            // 
            // 按编号搜索
            // 
            this.按编号搜索.Name = "按编号搜索";
            this.按编号搜索.Size = new System.Drawing.Size(148, 22);
            this.按编号搜索.Text = "按编号搜索";
            this.按编号搜索.Click += new System.EventHandler(this.按编号搜索ToolStripMenuItem_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoScroll = true;
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 25);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1370, 719);
            this.flowLayoutPanel3.TabIndex = 3;
            // 
            // WorkPlaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 744);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WorkPlaceForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.WorkPlaceForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 取消任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消所有任务;
        private System.Windows.Forms.ToolStripMenuItem 取消当前任务;
        private System.Windows.Forms.ToolStripMenuItem 微信ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 好友;
        private System.Windows.Forms.ToolStripMenuItem 通讯录加好友;
        private System.Windows.Forms.ToolStripMenuItem 朋友圈;
        private System.Windows.Forms.ToolStripMenuItem 通讯录定时加好友;
        private System.Windows.Forms.ToolStripMenuItem 搜索加好友;
        private System.Windows.Forms.ToolStripMenuItem 手动发朋友圈;
        private System.Windows.Forms.ToolStripMenuItem 定时发朋友圈;
        private System.Windows.Forms.ToolStripMenuItem 群ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 手动发群消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自动发群消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 闲鱼ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 粉丝;
        private System.Windows.Forms.ToolStripMenuItem 查看粉丝数;
        private System.Windows.Forms.ToolStripMenuItem 历史粉丝数;
        private System.Windows.Forms.ToolStripMenuItem 通讯里;
        private System.Windows.Forms.ToolStripMenuItem 检测通讯录;
        private System.Windows.Forms.ToolStripMenuItem 导入通讯录;
        private System.Windows.Forms.ToolStripMenuItem 回收数据;
        private System.Windows.Forms.ToolStripMenuItem 导入历史记录;
        private System.Windows.Forms.ToolStripMenuItem 辅助功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 解屏;
        private System.Windows.Forms.ToolStripMenuItem 锁屏;
        private System.Windows.Forms.ToolStripMenuItem 批量重启;
        private System.Windows.Forms.ToolStripMenuItem 批量关机;
        private System.Windows.Forms.ToolStripMenuItem 批量安装软件;
        private System.Windows.Forms.ToolStripMenuItem 更改输入法;
        private System.Windows.Forms.ToolStripMenuItem 历史任务;
        private System.Windows.Forms.ToolStripMenuItem 快手ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关注点赞评论ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按编号搜索;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    }
}

