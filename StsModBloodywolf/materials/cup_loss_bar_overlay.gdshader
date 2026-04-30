shader_type canvas_item;

// 底色：天蓝色（半透明，不影响血条本身可读性）
uniform vec4 base_color : source_color = vec4(0.53, 0.81, 0.98, 0.6);  // 天蓝 #87CEFA, alpha 0.6

// 条纹颜色：灰蓝色
uniform vec4 stripe_color : source_color = vec4(0.4, 0.6, 0.7, 0.8);   // 灰蓝，alpha略高增强条纹感

// 条纹移动速度
uniform float speed = 2.0;

// 条纹密度（值越大条纹越密）
uniform float stripe_density = 6.0;

void fragment() {
    // 使用 UV 坐标（0~1），乘以密度产生重复条纹
    vec2 uv = UV * stripe_density;
    
    // 随时间移动，产生斜向条纹（同时影响 x 和 y 方向）
    float t = TIME * speed;
    float pattern = sin( (uv.x + uv.y) * 3.14159 + t );
    
    // 将 pattern 从 [-1,1] 映射到 [0,1]，使条纹过渡平滑
    pattern = clamp(pattern * 0.7 + 0.5, 0.0, 1.0);
    
    // 根据 pattern 混合底色和条纹色
    COLOR = base_color;
    
    // 可选：让最终颜色完全保留混合后的 alpha（推荐保持半透明，让下方血条可见）
    // 如果希望更透明，可以乘一个全局 alpha，例如：
    // COLOR.a *= 0.7;
}