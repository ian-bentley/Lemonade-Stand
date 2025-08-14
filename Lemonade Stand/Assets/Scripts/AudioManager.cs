using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private AudioSource audio_source_2;
    [SerializeField] private AudioClip button_clip;
    [SerializeField] private AudioClip buy_clip;
    [SerializeField] private AudioClip pay_clip;
    [SerializeField] private AudioClip leave_clip;
    [SerializeField] private AudioClip increase_clip;
    [SerializeField] private AudioClip decrease_clip;
    [SerializeField] private AudioClip liked_clip;

    private void OnEnable() {
        UIButtonListener.OnMenuButtonClicked += PlayButtonSound;
        UIButtonListener.OnBuyButtonClicked += PlayBuySound;
        UIButtonListener.OnIncreaseButtonClicked += PlayIncreaseSound;
        UIButtonListener.OnDecreaseButtonClicked += PlayDecreaseSound;
        Player.OnPaid += PlayPaySound;
        Customer.OnCustomerLeft += (id) => PlayLeaveSound();
        //CustomerManager.OnCustomerServed += PlayLikedSound;
    }

    public void PlayButtonSound() {
        audio_source.clip = button_clip;
        audio_source.Play();
    }

    public void PlayBuySound() {
        audio_source.clip = buy_clip;
        audio_source.Play();
    }

    public void PlayPaySound() {
        audio_source.clip = pay_clip;
        audio_source.Play();
    }

    public void PlayLeaveSound() {
        audio_source.clip = leave_clip;
        audio_source.Play();
    }

    public void PlayIncreaseSound() {
        audio_source.clip = increase_clip;
        audio_source.Play();
    }

    public void PlayDecreaseSound() {
        audio_source.clip = decrease_clip;
        audio_source.Play();
    }

    public void PlayLikedSound() {
        audio_source_2.clip = liked_clip;
        audio_source_2.Play();
    }
}
