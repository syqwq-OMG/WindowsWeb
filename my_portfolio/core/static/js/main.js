// core/static/js/main.js
document.addEventListener('DOMContentLoaded', () => {
    gsap.registerPlugin(ScrollTrigger);

    gsap.from('.hero h1', { duration: 1, y: 30, opacity: 0, ease: 'power4.out', delay: 0.2 });
    gsap.from('.hero p', { duration: 1, y: 30, opacity: 0, ease: 'power4.out', delay: 0.4 });

    const projectCards = gsap.utils.toArray('.project-card');
    projectCards.forEach(card => {
        gsap.to(card, {
            opacity: 1,
            y: 0,
            duration: 0.8,
            ease: 'power3.out',
            scrollTrigger: {
                trigger: card,
                start: 'top 85%',
                toggleActions: 'play none none none',
            }
        });
    });
});