// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function() {
    // Apply animation to cards
    const animatedItems = document.querySelectorAll('.animated-item');
    if (animatedItems.length) {
        animatedItems.forEach((item, index) => {
            item.style.setProperty('--i', index + 1);
        });
    }
    
    // Multi-step form functionality
    const multiStepForm = document.getElementById('multiStepForm');
    if (multiStepForm) {
        const formSteps = document.querySelectorAll('.form-step');
        const stepItems = document.querySelectorAll('.step-item');
        const nextButtons = document.querySelectorAll('.btn-next-step');
        const prevButtons = document.querySelectorAll('.btn-prev-step');
        const submitButton = document.querySelector('.btn-submit');
        
        let currentStep = 0;
        
        // Initialize form
        updateFormState();
        
        // Next button event
        nextButtons.forEach(button => {
            button.addEventListener('click', function() {
                // Validate current step before proceeding
                if (validateStep(currentStep)) {
                    currentStep++;
                    updateFormState();
                    window.scrollTo(0, 0);
                }
            });
        });
        
        // Previous button event
        prevButtons.forEach(button => {
            button.addEventListener('click', function() {
                currentStep--;
                updateFormState();
                window.scrollTo(0, 0);
            });
        });
        
        // Function to update form display
        function updateFormState() {
            formSteps.forEach((step, index) => {
                step.classList.toggle('active', index === currentStep);
            });
            
            stepItems.forEach((item, index) => {
                item.classList.remove('active', 'completed');
                if (index < currentStep) {
                    item.classList.add('completed');
                } else if (index === currentStep) {
                    item.classList.add('active');
                }
            });
            
            // Show/hide navigation buttons
            prevButtons.forEach(btn => {
                btn.style.display = currentStep > 0 ? 'block' : 'none';
            });
            
            // Show submit button only on the last step
            if (submitButton) {
                submitButton.style.display = currentStep === formSteps.length - 1 ? 'block' : 'none';
            }
            
            // Show/hide next button except on the last step
            nextButtons.forEach(btn => {
                btn.style.display = currentStep < formSteps.length - 1 ? 'block' : 'none';
            });
        }
        
        // Basic validation function
        function validateStep(stepIndex) {
            const currentStepEl = formSteps[stepIndex];
            const requiredInputs = currentStepEl.querySelectorAll('input[required], select[required], textarea[required]');
            
            let isValid = true;
            
            requiredInputs.forEach(input => {
                if (!input.value.trim()) {
                    isValid = false;
                    input.classList.add('is-invalid');
                } else {
                    input.classList.remove('is-invalid');
                }
            });
            
            if (!isValid) {
                const errorMsg = currentStepEl.querySelector('.validation-message');
                if (errorMsg) {
                    errorMsg.textContent = 'Please fill in all required fields.';
                    errorMsg.style.display = 'block';
                }
            }
            
            return isValid;
        }
    }
    
    // Range slider labels
    const rangeSliders = document.querySelectorAll('.form-range');
    if (rangeSliders.length) {
        rangeSliders.forEach(slider => {
            const output = document.createElement('output');
            output.classList.add('range-value');
            slider.parentNode.appendChild(output);
            
            // Set initial value
            output.value = slider.value;
            
            // Update value on input event
            slider.addEventListener('input', function() {
                output.value = this.value;
            });
            
            // Position the output
            positionOutput(slider, output);
            
            // Update position on window resize
            window.addEventListener('resize', function() {
                positionOutput(slider, output);
            });
        });
        
        function positionOutput(slider, output) {
            const width = slider.offsetWidth;
            const value = slider.value;
            const max = slider.max || 10;
            const min = slider.min || 0;
            
            // Calculate position based on slider value
            const position = ((value - min) / (max - min)) * width;
            
            // Set output position
            output.style.left = `${position}px`;
        }
    }
    
    // Initialize charts if they exist
    initializeCharts();
});

// Charts initialization
function initializeCharts() {
    const symptomChart = document.getElementById('symptomChart');
    if (symptomChart) {
        let ctx = symptomChart.getContext('2d');
        
        // Get data from data attributes
        let labels = JSON.parse(symptomChart.dataset.labels || '[]');
        let values = JSON.parse(symptomChart.dataset.values || '[]');
        
        new Chart(ctx, {
            type: 'radar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Symptoms Severity',
                    data: values,
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 2,
                    pointBackgroundColor: 'rgba(75, 192, 192, 1)',
                    pointBorderColor: '#fff',
                    pointHoverBackgroundColor: '#fff',
                    pointHoverBorderColor: 'rgba(75, 192, 192, 1)'
                }]
            },
            options: {
                scales: {
                    r: {
                        beginAtZero: true,
                        max: 10,
                        ticks: {
                            stepSize: 2
                        }
                    }
                },
                plugins: {
                    legend: {
                        display: false
                    }
                },
                elements: {
                    line: {
                        tension: 0.2
                    }
                }
            }
        });
    }
    
    // Probability chart animation
    const probabilityFill = document.querySelector('.probability-fill');
    if (probabilityFill) {
        setTimeout(() => {
            probabilityFill.style.width = probabilityFill.dataset.value || '0%';
        }, 300);
    }
}

// Utility function to animate numbers
function animateNumber(element, target, duration = 1000) {
    let start = 0;
    const startTime = performance.now();
    
    function updateNumber(currentTime) {
        const elapsedTime = currentTime - startTime;
        if (elapsedTime > duration) {
            element.textContent = target;
            return;
        }
        
        const progress = elapsedTime / duration;
        const current = Math.round(progress * target);
        element.textContent = current;
        requestAnimationFrame(updateNumber);
    }
    
    requestAnimationFrame(updateNumber);
}
