document.addEventListener('click', async function (evt) {
    const target = evt.target;
    
    if (target.classList.contains('qty-plus') || target.classList.contains('qty-minus')) {
        const productId = parseInt(target.getAttribute('data-product-id'), 10);
        const row = document.querySelector(`[data-product-id="${productId}"]`);
        const qtySpan = row.querySelector('.qty-value');
        const currentQty = parseInt(qtySpan.textContent, 10);
        const newQty = target.classList.contains('qty-plus') ? currentQty + 1 : Math.max(currentQty - 1, 0)
        
        const response = await fetch('/cart/update', {
           method: 'POST',
           headers: {'Content-Type': 'application/json'},
           body: JSON.stringify({ productId, quantity: newQty })
        });
        const result = await response.json();
        
        if (result.success) {
            location.reload();
        } else {
            document.getElementById('cart-alert').textContent = result.message;
        }
    }
    
    if (target.classList.contains('qty-remove')) {
        const productId = parseInt(target.getAttribute('data-product-id'), 10);
        
        await fetch('/cart/remove', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({productId: productId})
        });
        
        location.reload();
    }
});