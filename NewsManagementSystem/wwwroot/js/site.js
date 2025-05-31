//Gửi thông tin qua tạo tk qua form
const formCreateNewAccount = document.querySelector(".form-create-account");

if (formCreateNewAccount) {
    formCreateNewAccount.addEventListener("submit", async (e) => {
        e.preventDefault();

        // Lấy dữ liệu từ form
        const formData = new FormData(formCreateNewAccount);
        const data = Object.fromEntries(formData.entries());

        console.log("Sending data:", data);
        const payload = {
            AccountID: data["CreateAccountRequest.AccountID"],
            AccountName: data["CreateAccountRequest.AccountName"],
            AccountEmail: data["CreateAccountRequest.AccountEmail"],
            AccountRole: data["CreateAccountRequest.AccountRole"],
            AccountPassword: data["CreateAccountRequest.AccountPassword"],
        };
        console.log("Sending payload:", payload);

        try {
            const response = await fetch("/SystemAccount/CreateSystemAccount", {
                headers: {
                    "Content-Type": "application/json",
                },
                method: "POST",
                body: JSON.stringify(payload)
            });
            if (response.ok) {
                window.location.reload();
            } else {
                const errorResult = await response.text();
                console.error("Error from server:", errorResult);
                let errorJson;
                try {
                    errorJson = JSON.parse(errorResult);
                } catch (ex) {
                    console.error("Không parse được JSON từ server:", errorResult);
                    return;
                }
                Object.keys(errorJson.errors).forEach(propName => {
                    const selector = `[data-valmsg-for="CreateAccountRequest.${propName}"]`;
                    const span = document.querySelector(selector);
                    if (span) {
                        span.innerText = errorJson.errors[propName];
                    } else {
                        console.warn("Không tìm thấy span để đẩy lỗi cho", propName);
                    }
                });
            }
        } catch (error) {
            console.error("Network error:", error);
        }
    });
}

//Gửi thông tin để update account bằng form
const formUpdateAccount = document.querySelector(".form-update-account");

if (formUpdateAccount) {
    formUpdateAccount.addEventListener("submit", async (e) => {
        e.preventDefault();
        const formData = new FormData(formUpdateAccount);
        const data = Object.fromEntries(formData.entries());
        console.log("Sending payload:", data);
        try {
            const response = await fetch("/SystemAccount/Edit", {
                headers: {
                    "Content-Type": "application/json",
                },
                method: "POST",
                body: JSON.stringify(data),
            });
            if (response.ok) {
                window.location.reload();
            } else {
                const errorResult = await response.text();
                console.error("Error from server:", errorResult);
                let errorJson;
                try {
                    errorJson = JSON.parse(errorResult);
                } catch (ex) {
                    console.error("Không parse được JSON từ server:", errorResult);
                    return;
                }
                Object.keys(errorJson.errors).forEach(propName => {
                    const selector = `[data-valmsg-for="UpdateSystemAccountRequest.${propName}"]`;
                    const span = document.querySelector(selector);
                    if (span) {
                        span.innerText = errorJson.errors[propName];
                    } else {
                        console.warn("Không tìm thấy span để đẩy lỗi cho", propName);
                    }
                });
            }
        } catch (error) {
            console.error("Network error:", error);
        }
        
    })
}
