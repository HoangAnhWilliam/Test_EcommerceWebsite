document.getElementById("courseReviewForm").addEventListener("submit", function (e) {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const rating = document.getElementById("rating").value;
    const comment = document.getElementById("comment").value;


    const review = document.createElement("div");
    review.classList.add("review");
    review.innerHTML = `<strong>${name} đánh giá: ${rating}</strong><p>${comment}</p>`;
    document.querySelector(".reviews").appendChild(review);


    document.getElementById("name").value = "";
    document.getElementById("rating").value = "5";
    document.getElementById("comment").value = "";
});
