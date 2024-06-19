using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class AnimatedButton : Button
{
    private Timer animationTimer;
    private float progress;
    private bool isPressed;
    private DateTime pressStartTime;
    private float animationDuration; // 애니메이션 지속 시간 (초)
    private int lineThickness; // 선 두께
    private string originalText;

    public AnimatedButton(float duration, int thickness)
    {
        // 타이머 설정
        animationTimer = new Timer();
        animationTimer.Interval = 16; // 16ms 간격으로 설정 (초당 60프레임)
        animationTimer.Tick += AnimationTimer_Tick;

        // 초기화
        progress = 0;
        isPressed = false;
        animationDuration = duration;
        lineThickness = thickness;
        originalText = this.Text; // 원래 텍스트 저장

        // 이벤트 핸들러 추가
        this.MouseDown += AnimatedButton_MouseDown;
        this.MouseUp += AnimatedButton_MouseUp;
        this.MouseLeave += AnimatedButton_MouseLeave;
        this.KeyDown += AnimatedButton_KeyDown;
        this.KeyUp += AnimatedButton_KeyUp;
        this.TextChanged += AnimatedButton_TextChanged;
        
    }

    private void AnimatedButton_TextChanged(object sender, EventArgs e)
    {
        if (!isPressed)
        {
            originalText = this.Text;
        }
    }

    private void StartPress()
    {
        if (!isPressed)
        {
            isPressed = true;
            progress = 0;
            pressStartTime = DateTime.Now;
            originalText = this.Text; // 원래 텍스트 저장
            this.Text = "클릭중.."; // 버튼 텍스트 변경
            animationTimer.Start();
        }
    }

    private void EndPress()
    {
        if (isPressed)
        {
            isPressed = false;
            this.Text = originalText; // 원래 텍스트로 복원
            animationTimer.Stop();
            progress = 0;
            this.Invalidate(); // 버튼을 다시 그립니다

            //이벤트 두번 발생함. 필요 없음. 
            //if ((DateTime.Now - pressStartTime).TotalSeconds >= animationDuration)
            //{
            //    OnClick(EventArgs.Empty); // 설정된 시간이 되면 클릭 이벤트 트리거
            //}
        }
    }

    private void AnimatedButton_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
        {
            EndPress();
        }
    }

    private void AnimatedButton_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
        {
            StartPress();
        }
    }
    private void AnimatedButton_MouseDown(object sender, MouseEventArgs e)
    {
        //isPressed = true;
        //progress = 0;
        //pressStartTime = DateTime.Now;
        //originalText = this.Text; // 원래 텍스트 저장
        //this.Text = "클릭중.."; // 버튼 텍스트 변경
        //animationTimer.Start();
        StartPress();
    }

    private void AnimatedButton_MouseUp(object sender, MouseEventArgs e)
    {
        EndPress();

        //if (isPressed && (DateTime.Now - pressStartTime).TotalSeconds >= animationDuration)
        //{
        //    OnClick(EventArgs.Empty); // 설정된 시간이 되면 클릭 이벤트 트리거
        //}

        //isPressed = false;
        //this.Text = originalText; // 원래 텍스트로 복원
        //animationTimer.Stop();
        //progress = 0;
        //this.Invalidate(); // 버튼을 다시 그립니다
    }

    private void AnimatedButton_MouseLeave(object sender, EventArgs e)
    {
        //isPressed = false;
        //this.Text = originalText; // 원래 텍스트로 복원
        //animationTimer.Stop();
        //progress = 0;
        //this.Invalidate(); // 버튼을 다시 그립니다
        EndPress();
        this.Text = originalText; // 원래 텍스트로 복원
    }

    private void AnimationTimer_Tick(object sender, EventArgs e)
    {
        if (isPressed)
        {
            progress = (float)(DateTime.Now - pressStartTime).TotalSeconds / animationDuration * GetPerimeter(); // 설정된 시간 동안 전체 둘레를 기준으로 진행

            if (progress >= GetPerimeter())
            {
                progress = GetPerimeter();
                animationTimer.Stop();
                OnClick(EventArgs.Empty); // 설정된 시간이 되면 클릭 이벤트 트리거
            }
            this.Invalidate(); // 버튼을 다시 그립니다
        }
    }

    private float GetPerimeter()
    {
        return 2 * (Width + Height - lineThickness); // 네변의 길이 합 계산
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if (isPressed)
        {
            // 네모난 애니메이션 그리기
            using (Pen pen = new Pen(Color.White, lineThickness)) // 선 두께를 설정된 값으로 설정
            {
                pen.StartCap = LineCap.Flat;
                pen.EndCap = LineCap.Flat;

                Rectangle rect = new Rectangle(lineThickness / 2, lineThickness / 2, this.Width - lineThickness, this.Height - lineThickness);

                float[] lengths = { rect.Width / 2, rect.Height / 2, rect.Height / 2, rect.Width / 2, rect.Width / 2, rect.Height /2 , rect.Height /2 , rect.Width / 2 }; // 각 변의 길이
                PointF[] points = {
                    new PointF(rect.Left + rect.Width / 2, rect.Top), // 시작점 (위쪽 중앙)
                    new PointF(rect.Right, rect.Top), // 우상단
                    new PointF(rect.Right, (rect.Top + rect.Bottom) /2), // 우중단
                    new PointF(rect.Right, rect.Bottom), // 우하단
                    new PointF(rect.Left + rect.Width / 2, rect.Bottom), // 아래 중앙
                    new PointF(rect.Left, rect.Bottom), // 좌하단
                    new PointF(rect.Left, (rect.Top + rect.Bottom) /2), // 좌 중단
                    new PointF(rect.Left, rect.Top), // 좌상단
                    new PointF(rect.Left + rect.Width / 2, rect.Top) // 끝점 (위쪽 중앙)

                    //new PointF(rect.Left + rect.Width / 2, rect.Top), // 시작점 (위쪽 중앙)
                    //new PointF(rect.Right, (rect.Top + rect.Bottom) /2), // 우상단
                    //new PointF(rect.Left + rect.Width / 2, rect.Bottom), // 시작점 (위쪽 중앙)
                    //new PointF(rect.Left, (rect.Top + rect.Bottom) /2), // 우상단
                    //new PointF(rect.Left, rect.Top), // 좌상단
                    //new PointF(rect.Left + rect.Width / 2, rect.Top) // 끝점 (위쪽 중앙)
                };

                using (GraphicsPath path = new GraphicsPath())
                {
                    float totalLength = 0;
                    for (int i = 0; i < lengths.Length; i++)
                    {
                        if (progress > totalLength + lengths[i])
                        {
                            path.AddLine(points[i], points[i + 1]);
                            totalLength += lengths[i];
                        }
                        else
                        {
                            float remainingLength = progress - totalLength;
                            if (remainingLength > 0)
                            {
                                PointF startPoint = points[i];
                                PointF endPoint = new PointF(
                                    points[i].X + (points[i + 1].X - points[i].X) * (remainingLength / lengths[i]),
                                    points[i].Y + (points[i + 1].Y - points[i].Y) * (remainingLength / lengths[i])
                                );

                                Console.WriteLine($"i:{i}, 총길이:{GetPerimeter()} progress:{progress}, totalLength:{totalLength}, remainingLength:{remainingLength} X:{endPoint.X}, Y:{endPoint.Y}");

                                path.AddLine(startPoint, endPoint);
                            }
                            break;
                        }
                    }

                    pevent.Graphics.DrawPath(pen, path);
                }
            }
        }
    }

    public bool IsPressedForDuration()
    {
        return (DateTime.Now - pressStartTime).TotalSeconds >= animationDuration;
    }
}
