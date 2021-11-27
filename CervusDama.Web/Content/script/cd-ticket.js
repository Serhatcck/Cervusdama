var ticketList = [];

document.addEventListener('DOMContentLoaded', function () {

	var ticketInput = document.getElementById('ticket-input');
	var ticketListContainer = document.getElementById('ticket-list');
	var ticketHiddenInput = document.getElementById('Tickets');

	ticketInput.addEventListener('keydown', function (e) {
		if (e.keyCode == 13) {
			e.preventDefault();

			var ticketText = e.target.value;
			ticketText = ticketText.trim().replace(/[^0-9a-zığüşöçİĞÜŞÖÇ\s]/gi, '');

			if (ticketText.length > 0) {
				if (!ticketList.includes(ticketText)) {
					ticketList.push(ticketText);
					ticketInput.value = '';

					var ticket = document.createElement('div');
					ticket.className = 'ticket';

					var span = document.createElement('span');
					span.innerHTML = ticketText;
					ticket.appendChild(span);

					var deleteLink = document.createElement('a');
					deleteLink.setAttribute('href', 'javascript:void(0)');
					deleteLink.addEventListener('click', function (e) {
						var parent = '';
						if (e.target.tagName === 'I') {
							parent = e.target.parentNode.parentNode;
						} else {
							parent = e.target.parentNode;
						}

						var deletedTicketText = parent.childNodes[0].innerHTML;
						var index = ticketList.indexOf(deletedTicketText);

						if (index > -1) {
							ticketList.splice(index, 1);
							ticketHiddenInput.value = ticketList.join(';');
						}

						ticketListContainer.removeChild(parent);
					});

					var deleteIcon = document.createElement('i');
					deleteIcon.className = 'cdi cdi-no-alt';
					deleteLink.appendChild(deleteIcon);
					ticket.appendChild(deleteLink);

					ticketListContainer.appendChild(ticket);
					ticketHiddenInput.value = ticketList.join(';');
				} else {
					console.log('Etiket zaten ekli..');
				}
			} else {
				console.log('Etiket ismi sadece karakter, sayı ve boşluktan oluşabilir.');
			}
		}
	});
});
